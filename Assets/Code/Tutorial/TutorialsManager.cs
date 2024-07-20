using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;

public class TutorialsManager : MonoBehaviour
{
    [SerializeField] private Transform tutorialSection;
    [SerializeField] private TutorialSequenceController fightingMinigameTutorial;
    
    private bool _fightingMinigameTutorialShown = false;
    public bool FightingMinigameTutorialShown => _fightingMinigameTutorialShown;

    public static TutorialsManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowFightingTutorial(Action onTutorialEndAction)
    {
        if (_fightingMinigameTutorialShown == true) return;
        _fightingMinigameTutorialShown = true;

        TutorialSequenceController tutorial = Instantiate(fightingMinigameTutorial, tutorialSection);
        tutorial.onTutorialEnd = onTutorialEndAction;
        tutorial.StartTutorial();
        
        EventsManager.Instance.SetSkippable(tutorial);
    }
}