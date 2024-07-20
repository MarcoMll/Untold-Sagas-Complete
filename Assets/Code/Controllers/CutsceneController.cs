using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour, ISkippable
{
    [SerializeField] private SceneComponentsHolder sceneComponents;
    [SerializeField] private EventsManager eventsManager;
    [SerializeField] private Transform cutsceneBehaviourHolder;
    [SerializeField] private ProgressBar nextEventProgressBar;

    private bool _allEventsPlayed = false;
    private Cutscene _cutscene;
    private CutsceneBehaviour _cutsceneBehaviour;

    private bool _cutsceneNotificationPlayed = false;

    private Coroutine _playNextEventCoroutine;
    
    public void OnSkipButtonPressed()
    {
       SkipEvent(); 
    }
    
    private IEnumerator PlayNextEventAfterDelay()
    {
        float delay = 10f;

        while (true)
        {
            Debug.Log($"Skip the event after {delay} second(s)");
            yield return new WaitForSeconds(delay);
            PlayCutsceneEvent();
        }
    }

    private void SkipEvent()
    {
        if (CanSkipEvent() == false)
        {
            return;
        }
        
        if (_playNextEventCoroutine != null)
        {
            StopCoroutine(_playNextEventCoroutine);
            _playNextEventCoroutine = null;
        }
        
        PlayCutsceneEvent();
        _playNextEventCoroutine = StartCoroutine(PlayNextEventAfterDelay());
    }
    
    private void ToggleGameInterface(bool value)
    {
        if (value == true)
        {
            UserInterfaceController.Instance.ShowAll();
        }
        else
        {
            UserInterfaceController.Instance.HideAll();
        }
    }

    private void InstantiateCutsceneBehaviour()
    {
        if (_cutsceneBehaviour == null)
        {
            CutsceneBehaviour cutsceneBehaviour = _cutscene.GetCutsceneBehaviour();
            if (cutsceneBehaviour == null) return;
            _cutsceneBehaviour = Instantiate(cutsceneBehaviour, cutsceneBehaviourHolder);
            _cutsceneBehaviour.gameObject.SetActive(false);
        }
    }
    
    private void ExecuteAnimations()
    {
        if (_cutsceneBehaviour == null)
        {
            return;
        }

        CutsceneEvent cutsceneEvent = _cutscene.GetCurrentCutsceneEvent();
        if (cutsceneEvent.ExecuteAnimations())
        {
            if (_cutsceneBehaviour.gameObject.activeSelf == false)
            {
                _cutsceneBehaviour.gameObject.SetActive(true);
            }

            _cutsceneBehaviour.ExecuteAnimations();
        }
    }

    private void TryToLoadChapter()
    {
        Chapter chapter = _cutscene.GetChapterToLoad();
        if (chapter != null)
        {
            eventsManager.SetChapter(chapter);
        }
    }

    private void ResetCutsceneBehaviour()
    {
        if (_cutsceneBehaviour == null) return;

        Destroy(_cutsceneBehaviour.gameObject);
        _cutsceneBehaviour = null;
    }

    private bool CanSkipEvent()
    {
        if (_cutsceneBehaviour == null)
        {
            return true;
        } 
        else if (_cutscene == null)
        {
            return false;
        }
        else
        {
            CutsceneEvent cutsceneEvent = _cutscene.GetCurrentCutsceneEvent();
            return !cutsceneEvent.ExecuteAnimations();
        }
    }
    
    public void PlayCutsceneEvent()
    {
        if (HasCutsceneToVisualize() == false)
        {
            if (_playNextEventCoroutine != null)
            {
                StopCoroutine(_playNextEventCoroutine);
                _playNextEventCoroutine = null;
            }

            return;
        }

        if (_cutsceneNotificationPlayed == false)
        {
            //TooltipController.Instance.InstantiateNotificationTooltip("Пропуск сцены","Вы можете пропустить сцену нажав на клавишу «Пробел»\nЕсли в сцене присутствуют анимации, ее пропуск невозможен", 
            //    new Vector3(0.5f, 0.5f, 0f), selfDestroyTime: 5f);
            _cutsceneNotificationPlayed = true;
        }
        
        if (_allEventsPlayed)
        {
            TryToLoadChapter();

            int targetEventID = _cutscene.GetTargetGameEventID();
            _cutscene = null;
            _allEventsPlayed = false;

            eventsManager.SetEventID(targetEventID);
            eventsManager.PlayEvent();
            ResetCutsceneBehaviour();
            ToggleGameInterface(true);

            if (_playNextEventCoroutine != null)
            {
                StopCoroutine(_playNextEventCoroutine);
                _playNextEventCoroutine = null;
            }

            return;
        }

        CutsceneEvent cutsceneEvent = _cutscene.GetCurrentCutsceneEvent();
        if (_cutscene.IsLastEvent(cutsceneEvent))
        {
            _allEventsPlayed = true;
        }

        sceneComponents.TitleField.WriteText(cutsceneEvent.Text);
        sceneComponents.SetBackground(cutsceneEvent.Background);
        sceneComponents.SetupDialogueCharacter(cutsceneEvent.DialogueCharacter, cutsceneEvent.Dialogue, Emotion.Idle, Color.white);

        if (cutsceneEvent.SetMusic())
        {
            sceneComponents.SetSong(cutsceneEvent.SongClip);
        }

        if (cutsceneEvent.SetAmbience())
        {
            sceneComponents.SetAmbience(cutsceneEvent.AmbienceClip);
        }

        if (cutsceneEvent.PlayAdditionalAudio())
        {
            sceneComponents.PlaySoundEffect(cutsceneEvent.AdditionalAudioClip);
        }

        InstantiateCutsceneBehaviour();
        ToggleGameInterface(false);
        ExecuteAnimations();
        _cutscene.SetNextCutsceneEvent();
    }

    public void SetCutscene(Cutscene newCutscene)
    {
        _cutscene = newCutscene;
    }

    public bool HasCutsceneToVisualize()
    {
        return _cutscene != null;
    }
}
