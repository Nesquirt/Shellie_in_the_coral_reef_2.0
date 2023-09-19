using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("- - - - - AUDIO SOURCE - - - - - ")]
    [SerializeField] AudioSource SoundtrackSource;
    [SerializeField] AudioSource MovementSource;
    [SerializeField] AudioSource SFXSource;

    [Header("- - - - - AUDIO CLIP - - - - - ")]
    public AudioClip bubble;
    public AudioClip soundtrack;

    private void Start()
    {
        SoundtrackSource.clip = soundtrack;
        SoundtrackSource.Play();
        MovementSource.clip = bubble;
        MovementSource.Play();
    }
}
