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
    [SerializeField] AudioSource GrowingCoralSource;
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
    public AudioClip Crush_Spawn;

    [Header("- - - - AUDIO CLIP SARA'S GAME (ONE SHOT)")]
    public AudioClip KeyTaken;
    public AudioClip CageOpening;

    [Header("- - - - AUDIO CLIP STEFANO'S GAME (ONE SHOT)")]
    public AudioClip GridDrop;
    public AudioClip TrashMoving;
    public AudioClip ShipHorn;
    public AudioClip GridClimb;

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
        SFXSource.clip = sfx;
        SFXSource.Play();
    }
}
