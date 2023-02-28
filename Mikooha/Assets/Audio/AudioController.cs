using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDirection = 1f;
    public float fadeDuration = 0.1f;
    private float playbackTime;
    private float defaultVolume = 1f;

    private void Start()
    {
        audioSource.time = DataHolder.GetFloat(audioSource.clip.name);
        playbackTime = audioSource.time;
        defaultVolume = audioSource.volume;
        audioSource.Pause();
        Invoke(nameof(FadeOut), 0.5f);
    }

    public void FadeIn()
    {
        FadeIn(fadeDuration);
    }

    public void FadeIn(float duration)
    {
        if (audioSource.isPlaying)
        {
            audioSource.volume = defaultVolume;
            fadeDuration = duration;
            fadeDirection = -1f;
        }
    }
    
    public void FadeOut()
    {
        FadeOut(fadeDuration);
    }

    public void FadeOut(float duration)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.volume = 0;
            fadeDuration = duration;
            fadeDirection = 1f;
        }
    }

    private void Fade()
    {
        if (audioSource.volume > defaultVolume && fadeDirection > 0)
            return;

        if (audioSource.volume == 0 && fadeDirection < 0)
            return;

        audioSource.volume += fadeDirection * fadeDuration * Time.deltaTime;

        if (audioSource.volume == 0)
            audioSource.Pause();
    }
    
    private void Update()
    {
        if (audioSource.isPlaying)
            playbackTime = audioSource.time;
        
        Fade();
    }
    private void OnDestroy()
    {
        DataHolder.AddFloat(audioSource.clip.name, playbackTime);
    }
}
