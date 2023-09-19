using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("- - - - AUDIO SOURCE")]
    [SerializeField] AudioSource SoundtrackSource;
    [SerializeField] AudioSource MovementSource;
    [SerializeField] AudioSource BubbleSource;
    [SerializeField] AudioSource CoralSource;
    [SerializeField] AudioSource SFXSource;

    [Header("- - - - AUDIO CLIP (LOOP)")]
    public AudioClip soundtrack;
    public AudioClip movement;
    public AudioClip bubble;

    [Header("- - - - AUDIO CLIP (ONE SHOT)")]
    public AudioClip coral;

    [Header("- - - - AUDIO CLIP MATTEO'S GAME (ONE SHOT)")]
    public AudioClip startRace;
    public AudioClip endRace;
    public AudioClip crossRing;


    private void Start()
    {
        SoundtrackSource.clip = soundtrack;
        SoundtrackSource.Play();
        MovementSource.clip = movement;
        MovementSource.Play();
        BubbleSource.clip = bubble;
        BubbleSource.Play();
    }
    public void PlaySFX(AudioClip sfx)
    {
        SFXSource.PlayOneShot(sfx);
    }
}
