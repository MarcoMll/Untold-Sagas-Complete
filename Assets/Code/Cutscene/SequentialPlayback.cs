using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialPlayback : CutsceneBehaviour
{
    [SerializeField] private AnimatableGroup[] animatableGroups;

    private int _currentAnimatableObjectIndex = 0;

    private void OnValidate()
    {
        foreach (AnimatableGroup animatableGroup in animatableGroups)
        {
            int index = System.Array.IndexOf(animatableGroups, animatableGroup);
            animatableGroup.SetGroupName("Group_" + index);
        }
    }

    public override void ExecuteAnimations()
    {
        ClearAnimationQueue();

        if (_currentAnimatableObjectIndex < animatableGroups.Length)
        {
            AnimatableGroup.AnimatableObject[] animatableObjects = animatableGroups[_currentAnimatableObjectIndex]
                .animatableObjects;

            for (int i = 0; i < animatableObjects.Length; i++)
            {
                AnimatableGroup.AnimatableObject animatableObject = animatableObjects[i];
                if (animatableObject.PlayAfter)
                {
                    QueueAnimation(animatableObject);
                }
                else
                {
                    animatableObject.OnExecuteAnimation.Invoke();
                }
            }
        }

        _currentAnimatableObjectIndex++;
    }
}