using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineQueue
{
    private Queue<IEnumerator> _coroutines = new Queue<IEnumerator>();
    private Coroutine _currentCoroutine = null;

    private MonoBehaviour _queueHolder;
    
    public Action onQueueEmpty;

    public CoroutineQueue(MonoBehaviour queueHolder)
    {
        _queueHolder = queueHolder;
    }
    
    private void StartNextCoroutine()
    {
        if (_queueHolder == null)
        {
            Debug.LogError("The queue holder was destroyed but the script still trying to continue the queue!");
            return;
        }
        
        if (_coroutines.Count > 0)
        {
            IEnumerator coroutine = RunCoroutine(_coroutines.Dequeue());
            _currentCoroutine = _queueHolder.StartCoroutine(coroutine);
        }
        else
        {
            onQueueEmpty?.Invoke();
        }
    }

    private IEnumerator RunCoroutine(IEnumerator coroutine)
    {
        yield return _queueHolder.StartCoroutine(coroutine);
        _currentCoroutine = null;
        StartNextCoroutine();
    }
    
    public void ClearQueue()
    {
        StopCurrentCoroutine();
        _coroutines.Clear();
    }

    public void StopCurrentCoroutine(bool startNext = false)
    {
        if (_currentCoroutine == null) return;
            
        _queueHolder.StopCoroutine(_currentCoroutine);
        _currentCoroutine = null;

        if (startNext == false) return;
        StartNextCoroutine();
    }
    
    public void EnqueueCoroutine(IEnumerator coroutine)
    {
        _coroutines.Enqueue(coroutine);
    }

    public void StartQueue()
    {
        StartNextCoroutine();
    }
}