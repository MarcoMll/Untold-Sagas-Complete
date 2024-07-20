using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtilities;

[RequireComponent(typeof(CanvasGroupController))]
public class Book : MonoBehaviour
{
    [SerializeField] private float changePageDuration = 0.3f;
    [SerializeField] private AudioClip changePageClip;
    
    private Page _currentPage;
    private Page _currentSubpage;
    private CanvasGroupController _canvasGroupController;

    private void Awake()
    {
        _canvasGroupController = GetComponent<CanvasGroupController>();
    }

    private void DeactivateCurrentPage()
    {
        if (_currentPage != null)
        {
            _currentPage.Hide(changePageDuration);
            _currentPage = null;
        }

        if (_currentSubpage != null)
        {
            _currentSubpage.Hide(changePageDuration);
            _currentSubpage = null;
        }
    }
    
    public void ActivatePage(Page targetPage)
    {
        DeactivateCurrentPage();
        
        EventsManager.Instance.SceneComponents.PlaySoundEffect(changePageClip);
        targetPage.Show(changePageDuration);
        _currentPage = targetPage;
    }

    public void ActivateSubpage(Page targetSubpage)
    {
        if (_currentSubpage != null)
            _currentSubpage.Hide(changePageDuration);
        
        targetSubpage.Show(changePageDuration);
        _currentSubpage = targetSubpage;
    }
    
    public void OpenBook()
    {
        UserInterfaceController.Instance.HideAll();
        _canvasGroupController.SmoothlyChangeAlpha(1f, changePageDuration);
        _canvasGroupController.CanvasGroup.blocksRaycasts = true;
        
        if (_currentPage != null) 
            _currentPage.Show(changePageDuration);
    }

    public void CloseBook()
    {
        UserInterfaceController.Instance.ShowAll();
        _canvasGroupController.SmoothlyChangeAlpha(0f, changePageDuration);
        _canvasGroupController.CanvasGroup.blocksRaycasts = false;
        
        DeactivateCurrentPage();
    }
}