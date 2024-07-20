using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0, 1)] private float initialVolume;

    public AudioClip Clip => audioSource.clip;
    
    private IEnumerator SmoothClipChange(AudioClip clip)
    {
        float elapsedTime = 0f;
        float smoothness = .5f;

        while (elapsedTime < smoothness)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / smoothness);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = clip;

        elapsedTime = 0f;

        while (elapsedTime < smoothness)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, initialVolume, elapsedTime / smoothness);
            yield return null;
        }

        audioSource.volume = initialVolume;

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

        yield break;
    }

    private void Play(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("Audio Source is null! Please, assign a component.");
            return;
        }
        
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayAudioClip(AudioClip clip, bool instant = false)
    {
        if (instant == true)
        {
            Play(clip);
        }
        else
        {
            StartCoroutine(SmoothClipChange(clip));
        }
    }

    public void OverlapAudioClip(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("Audio Source is null! Please, assign a component.");
            return;
        }
        
        audioSource.PlayOneShot(clip);
    }
}
