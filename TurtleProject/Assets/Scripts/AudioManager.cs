using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioMixer myMixer;
    private AudioMixerGroup Music, SFX;
    private List<AudioSource> audioSources;
    private Dictionary<string, AudioClip> audioClips;

    public void Awake()
    {
        SFX = myMixer.FindMatchingGroups("SFX")[0];
        Music = myMixer.FindMatchingGroups("Music")[0];
        audioSources = new();
        audioClips = new();
        LoadAudioClips("Sounds");
        //Simulo l'inizio del gioco
        GameMusic();
    }
    public void Update()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.gameObject.SetActive(false);
            }
        }
    }
    public void LoadAudioClips(String folderPath)
    {
        AudioClip[] temp = Resources.LoadAll<AudioClip>(folderPath);
        foreach (AudioClip clip in temp)
        {
            Debug.Log(clip.name);
            audioClips.Add(clip.name, clip);
        }
    }
    public void MenuMusic()
    {
        Play("menu_Music", true, 1f);
    }
    public void GameMusic()
    {
        StartCoroutine(FadeTwoClips("menu_Music", 0f, "freeRoaming_Music", 1f, 3f));
        Play("water_SFX", true, 1f);
        Play("bubble_SFX", true, 1f);
    }
    private AudioSource GetAvailableSource(String clipName)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.gameObject.name = clipName;
                return source;
            }
        }
        // If no available source found, create a new one
        GameObject newAudioSource = new(clipName);
        newAudioSource.transform.parent = transform;
        AudioSource newSource = newAudioSource.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        return newSource;
    }
    private void SetMixerGroup(AudioSource source, String clipName)
    {
        if (clipName.EndsWith("_SFX")) source.outputAudioMixerGroup = SFX;
        else source.outputAudioMixerGroup = Music;
    }
    private AudioSource GetAudioSourceFromClip(AudioClip clip)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == clip)
            {
                return audioSource;
            }
        }
        return null;
    }
    public void Play(String clipName, bool loop, float volume)
    {
        if (!audioClips.ContainsKey(clipName))
        {
            Debug.LogWarning("Clip audio non trovata: " + clipName);
            return;
        }
        else
        {
            AudioClip audioClip = audioClips[clipName];
            AudioSource source = GetAvailableSource(clipName);
            if (source != null)
            {
                source.gameObject.SetActive(true);
                source.clip = audioClip;
                SetMixerGroup(source, clipName);
                source.loop = loop;
                source.volume = volume;
                source.Play();
            }
            else
            {
                Debug.LogWarning("Audiosource nullo!");
            }
        }
    }
    public IEnumerator FadeTwoClips(String clip1Name, float targetVolume1, String clip2Name, float targetVolume2, float duration)
    {
        StartCoroutine(Fade(clip1Name, targetVolume1, duration));
        StartCoroutine(Fade(clip2Name, targetVolume2, duration));

        yield return new WaitForSeconds(duration);
    }
    public IEnumerator Fade(String clipName, float targetVolume, float duration)
    {
        if (!audioClips.ContainsKey(clipName))
        {
            Debug.LogWarning("Clip audio non trovata: " + clipName);
            yield break;
        }
        else
        {
            AudioClip audioClip = audioClips[clipName];
            AudioSource source = GetAudioSourceFromClip(audioClip);
            float startVolume;

            if (source == null || !source.isPlaying)
            {
                Play(audioClip.name, true, 0);
                startVolume = 0;
                source = GetAudioSourceFromClip(audioClip);
            }
            else
            {
                SetMixerGroup(source, clipName);
                startVolume = source.volume;
            }
            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                source.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / duration);
                yield return null;
            }

            source.volume = targetVolume;

            if (targetVolume == 0f)
            {
                source.Stop();
            }
        }

    }
}