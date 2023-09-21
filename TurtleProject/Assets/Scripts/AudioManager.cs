using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    [Header("- - - - AUDIO SOURCE")]
    [SerializeField] AudioSource BackgroundMusicSource;
    [SerializeField] AudioSource MinigamesSoundtrackSource;
    [SerializeField] AudioSource MovementSource;
    [SerializeField] AudioSource BubbleSource;
    [SerializeField] AudioSource GrowingCoralSource;
    [SerializeField] AudioSource TrashMovingSource;
    [SerializeField] AudioSource SFXSource;


    [Header("- - - - AUDIO CLIP (LOOP)")]
    public AudioClip backgroundMusic;
    public AudioClip movement;
    public AudioClip bubble;

    [Header("- - - - AUDIO CLIP (ONE SHOT)")]
    public AudioClip coral;
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
    /*public void ChangeMusic(AudioClip miniGameSoundtrack)
    {
        SoundtrackSource.clip = miniGameSoundtrack;
        SoundtrackSource.Play();
        StartCoroutine(Fade());
    }
    public IEnumerator Fade()
    {
        float time = 0f;
        float duration = 5f;
        while (time < duration)
        {
            time += Time.deltaTime;
            SoundtrackSource.volume = Mathf.Lerp(0.2f, 0, time / duration);
            MinigamesSoundtrackSource.volume = Mathf.Lerp(0, 0.2f, time / duration);
            yield return null;
        }
        yield break;
    }*/

}