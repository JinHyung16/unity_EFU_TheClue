using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(AudioClip clip)
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}
