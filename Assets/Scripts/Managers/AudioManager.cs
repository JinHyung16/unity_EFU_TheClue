using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    [Header("SFX Clip")]
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private int sfxChannles;
    private AudioSource[] sfxSources;

    private float sfxVolume = 1.0f;

    /// <summary>
    /// Audio Clip에 넣은 순서와 똑같이 사용해야 Index 번호가 맞는다.
    /// </summary>
    public enum SFX
    {
        PlayerMove = 0,
        UIClick,
        DialogueBtn,
        GameResult_Celar,
        GAmeResult_Fail,
        LightSwitch,
        DoorLock_PushButton,
        DoorLock_Success,
        DoorLock_Fail,
        TileMatch_Succes,
        ThemeThird_DropKey,
        ThemeThird_EnemyAttack,
        ThemeThird_RegionButton,
        ThemeThird_GradStudent_Tired,
    }
    private void Start()
    {
        InitAudioManager();
    }

    private void InitAudioManager()
    {
        GameObject commObj = new GameObject("Common SFX Player");
        commObj.transform.parent = this.transform;
        sfxSources = new AudioSource[sfxChannles];
        for (int i = 0; i < sfxSources.Length; i++)
        {
            sfxSources[i] = commObj.AddComponent<AudioSource>();
            sfxSources[i].clip = sfxClips[i];
            sfxSources[i].playOnAwake = false;
            sfxSources[i].bypassListenerEffects = true;
            sfxSources[i].volume = sfxVolume;
            sfxSources[i].loop = false;
        }
    }

    public void PlaySFX(SFX sfx)
    {
        if (sfx == SFX.PlayerMove)
        {
            sfxVolume = 0.3f;
            if (!sfxSources[(int)sfx].isPlaying)
            {
                sfxSources[(int)sfx].volume = sfxVolume;
                sfxSources[(int)sfx].Play();
            }
        }
        else
        {
            sfxVolume = 0.8f;
            sfxSources[(int)sfx].volume = sfxVolume;
            sfxSources[(int)sfx].Play();
        }

        /*
        //Debug.Log(sfx + " 호출");
        for (int i = 0; i < sfxSources.Length; i++)
        {
            int loopIndex = (i + sfxChannles) % sfxSources.Length;

            if (sfxSources[loopIndex].isPlaying)
            {
                Debug.Log(sfx + " 이미 실행중");
                continue;
            }
            sfxChannles = loopIndex;
            sfxSources[loopIndex].clip = sfxClips[(int)sfx];
            sfxSources[loopIndex].volume = sfxVolume;
            sfxSources[loopIndex].Play();
            break;
        }
        */
    }
}
