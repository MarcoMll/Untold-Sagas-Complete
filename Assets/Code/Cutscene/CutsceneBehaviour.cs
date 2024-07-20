using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CustomInspector;
using UnityEngine.Serialization;

public abstract class CutsceneBehaviour : MonoBehaviour
{
    private List<Coroutine> _animationCoroutines = new List<Coroutine>();
    
    private IEnumerator PlayAnimationAfterDelay(AnimatableGroup.AnimatableObject animatable)
    {
        yield return new WaitForSeconds(animatable.Delay);
        animatable.OnExecuteAnimation.Invoke();
    }

    protected void ClearAnimationQueue()
    {
        for (int i = 0; i < _animationCoroutines.Count; i++)
        {
            StopCoroutine(_animationCoroutines[i]);
        }
        
        _animationCoroutines.Clear();
    }

    protected void QueueAnimation(AnimatableGroup.AnimatableObject animatableObject)
    {
        Coroutine animationCoroutine = StartCoroutine(PlayAnimationAfterDelay(animatableObject));
        _animationCoroutines.Add(animationCoroutine);
        Debug.Log($"{animatableObject.ToString()} was added to the animation queue and will be executed in {animatableObject.Delay} second(s).");
    }

    public abstract void ExecuteAnimations();
}

[System.Serializable]
public class AnimatableGroup
{
    [HideInInspector] public string GroupName = "Group_1";
    public AnimatableObject[] animatableObjects;

    public void SetGroupName(string name)
    {
        GroupName = name;
    }

    [System.Serializable]
    public class AnimatableObject
    {
        [SerializeField] private string ObjectName = "Object_1";

        public bool PlayAfter;
        [ShowIf("PlayAfter")] public float Delay;

        [HorizontalLine("Animations Setup")] public UnityEvent OnExecuteAnimation;

        public override string ToString()
        {
            return ObjectName;
        }
    }
}