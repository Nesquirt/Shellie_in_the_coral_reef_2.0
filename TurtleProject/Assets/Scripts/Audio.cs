using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("- - - - AUDIO SOURCE - - - - - ")]
    [SerializeField] AudioSource SoundtrackSource;
    [SerializeField] AudioSource MovementSource;
    [SerializeField] AudioSource BubbleSource;
    [SerializeField] AudioSource SFX2DSource;
    [SerializeField] AudioSource SFX3DSource;

    [Header("- - - - - AUDIO CLIP - - - - - - ")]
    public AudioClip soundtrack;
    public AudioClip movement;
    public AudioClip bubble;

    private void Start()
    {
        SoundtrackSource.clip = soundtrack;
        SoundtrackSource.Play();
        MovementSource.clip = movement;
        MovementSource.Play();
        BubbleSource.clip = bubble;
        BubbleSource.Play();
    }
}
