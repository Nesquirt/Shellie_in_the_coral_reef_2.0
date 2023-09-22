using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    [Header("- - - - AUDIO SOURCE")]
    [SerializeField] private AudioSource BackgroundMusicSource;
    [SerializeField] private AudioSource MinigamesSoundtrackSource;
    [SerializeField] private AudioSource MovementSource;
    [SerializeField] private AudioSource BubbleSource;
    [SerializeField] private AudioSource TrashMovingSource;
    [SerializeField] private AudioSource SFXSource;


    [Header("- - - - AUDIO CLIP (LOOP)")]
    public AudioClip backgroundMusic;
    public AudioClip movement;
    public AudioClip bubble;

    [Header("- - - - AUDIO CLIP (ONE SHOT)")]
    public AudioClip selection;
    public AudioClip endMiniGame;

    [Header("- - - - AUDIO CLIP MATTEO'S GAME")]
    public AudioClip MatteoGameSountrack;
    public AudioClip startRace;
    public AudioClip crossRing;
    public AudioClip Crush_Spawn;

    [Header("- - - - AUDIO CLIP SARA'S GAME")]
    public AudioClip SaraGameSountrack;
    public AudioClip KeyTaken;
    public AudioClip CageOpening;

    [Header("- - - - AUDIO CLIP STEFANO'S GAME")]
    public AudioClip StefanoGameSountrack;
    public AudioClip GridDrop;
    public AudioClip trashMoving;
    public AudioClip ShipHorn;
    public AudioClip GridClimb;

    private void Start()
    {
        BackgroundMusicSource.clip = backgroundMusic;
        BackgroundMusicSource.Play();
        MovementSource.clip = movement;
        MovementSource.Play();
        BubbleSource.clip = bubble;
        BubbleSource.Play();
    }
    public void PlaySFX(AudioClip sfx)
    {
        SFXSource.clip = sfx;
        SFXSource.Play();
    }
    public void PlayTrash()
    {
        TrashMovingSource.clip = trashMoving;
        TrashMovingSource.Play();
    }
    public void ChangeMusic(AudioClip miniGameSoundtrack,bool fade,float vol)
    {
        StartCoroutine(Fade(miniGameSoundtrack,fade,vol));
    }
    public IEnumerator Fade(AudioClip miniGameSoundtrack,bool fade, float vol)
    {
        float time = 0f;
        float duration = 5f;
        if(fade)
        {
            MinigamesSoundtrackSource.clip = miniGameSoundtrack;
            MinigamesSoundtrackSource.Play();
            while (time < duration)
            {
                time += Time.deltaTime;
                BackgroundMusicSource.volume = Mathf.Lerp(0.2f, 0, time / duration);
                MinigamesSoundtrackSource.volume = Mathf.Lerp(0, vol, time / duration);
                yield return null;
            }
        }
        else
        {
            while (time < duration)
            {
                time += Time.deltaTime;
                BackgroundMusicSource.volume = Mathf.Lerp(0, 0.2f, time / duration);
                MinigamesSoundtrackSource.volume = Mathf.Lerp(vol, 0, time / duration);
                yield return null;
            }
            MinigamesSoundtrackSource.Play();
        }
    }

}