using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUtilities
{
    public class TimeUtility
    {
        private class CoroutineHolder : MonoBehaviour { }

        private static CoroutineHolder _coroutineHolder;

        private static CoroutineHolder coroutineHolder
        {
            get
            {
                if (_coroutineHolder == null)
                {
                    GameObject holderObject = new GameObject("CoroutineHolder");
                    UnityEngine.Object.DontDestroyOnLoad(holderObject);
                    _coroutineHolder = holderObject.AddComponent<CoroutineHolder>();
                }
                return _coroutineHolder;
            }
        }

        public static Coroutine Start(IEnumerator coroutine)
        {
            return coroutineHolder.StartCoroutine(coroutine);
        }

        public static void Stop(Coroutine coroutine)
        {
            coroutineHolder.StopCoroutine(coroutine);
        }

        public static Coroutine CallWithDelay(float delay, Action callableMethod)
        {
            //Debug.Log($"Action registered and will be played in {delay} second(s).");
            return Start(_CallWithDelay(delay, callableMethod));
        }

        private static IEnumerator _CallWithDelay(float delay, Action callableMethod)
        {
            yield return new WaitForSeconds(delay);
            callableMethod?.Invoke();
        }  
    }
}
