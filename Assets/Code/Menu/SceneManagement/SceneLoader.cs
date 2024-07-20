using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomInspector;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CanvasGroupController))]
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TMP_Text titleTextField;
    [SerializeField] private TextVisualizer textArea;
    [SerializeField] private DynamicImage backgroundImage;
    [SerializeField] private CanvasGroupController completenessText;
    [SerializeField] private LoadingScreen[] loadingScreens;

    private AsyncOperation _asyncLoad;
    private bool _allowedToLoadScene = false;
    private int _currentLoadingScreenIndex = -1; // Track the current loading screen index
    
    [Serializable]
    public class LoadingScreen
    {
        [SerializeField] private string title;
        [SerializeField, TextArea(5,10)] private string loadingText;
        [SerializeField, Preview(Size.medium)] private Sprite backgroundSprite;

        public string Title => title;
        public string Text => loadingText;
        public Sprite Background => backgroundSprite;
    }
    
    private CanvasGroupController _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroupController>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (_allowedToLoadScene)
            {
                StopCoroutine(ChangeLoadScreenAfterDelay());
                _asyncLoad.allowSceneActivation = true;
            }
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the main scene
        _asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        _asyncLoad.allowSceneActivation = false;

        // Update the progress bar
        while (!_asyncLoad.isDone)
        {
            progressBar.SetCurrentValue(_asyncLoad.progress);

            // Check if the load has finished
            if (_asyncLoad.progress >= 0.9f)
            {
                progressBar.SetCurrentValue(1f);
                _allowedToLoadScene = true;
                completenessText.SmoothlyChangeAlpha(1f, .5f);
                Debug.Log("Game scene ready to load.");
            }

            yield return null;
        }
    }

    private IEnumerator ChangeLoadScreenAfterDelay()
    {
        while (true)
        {
            ChangeLoadScreen();
            yield return new WaitForSeconds(25f);
        }
    }

    private void ChangeLoadScreen()
    {
        int newLoadingScreenIndex;
        do
        {
            newLoadingScreenIndex = Random.Range(0, loadingScreens.Length);
        } while (newLoadingScreenIndex == _currentLoadingScreenIndex);

        _currentLoadingScreenIndex = newLoadingScreenIndex;
        LoadingScreen loadingScreen = loadingScreens[_currentLoadingScreenIndex];
        titleTextField.text = loadingScreen.Title;
        textArea.WriteText(loadingScreen.Text);
        backgroundImage.SmoothlyChangeImage(loadingScreen.Background);
    }
    
    public void Show()
    {
        _canvasGroup.SmoothlyChangeAlpha(1f, .5f);
        _canvasGroup.CanvasGroup.blocksRaycasts = true;
    }
    
    public void LoadGameScene()
    {
        StartCoroutine(ChangeLoadScreenAfterDelay());
        StartCoroutine(LoadSceneAsync("Game Scene"));
    }
}
