using UnityEngine;

public interface IAnimatable
{
    void ExecuteAnimation(float delay = 0f);
    bool AnimationFinished();
}
