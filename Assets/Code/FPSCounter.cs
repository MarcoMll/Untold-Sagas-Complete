using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsTextField;

    private int _lastFrameIndex = 0;
    private float[] _frameDeltaTimeArray;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _frameDeltaTimeArray = new float[50];
    }

    private void Update()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;

        float currentFramesPerSecond = CalculateAverageFPS();
        UpdateTextField(currentFramesPerSecond);
    }

    private float CalculateAverageFPS()
    {
        float totalDeltaTime = 0f;
        foreach (var frameDeltaTime in _frameDeltaTimeArray)
        {
            totalDeltaTime += frameDeltaTime;
        }

        float averageDeltaTime = totalDeltaTime / _frameDeltaTimeArray.Length;
        return 1.0f / averageDeltaTime;
    }

    private void UpdateTextField(float fpsAmount)
    {
        fpsTextField.text = $"{Mathf.RoundToInt(fpsAmount)} FPS";
    }
}