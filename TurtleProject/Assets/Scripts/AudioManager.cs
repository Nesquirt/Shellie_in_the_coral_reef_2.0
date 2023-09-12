using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicAudioSource;
    private AudioSource soundEffectsAudioSource;

    [SerializeField] private AudioSource BackgroundMUSIC, TenseMUSIC;
    [SerializeField] private float MUSIC_VOLUME, EFX_VOLUME; //da 0 a 1
    [SerializeField] private AudioMixerGroup MUSIC_MIXER, EFX_MIXER;

    public GameObject slider;
    //private AudioMixer MUSIC_MIXER, EFX_MIXER;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantieni l'AudioManager tra le scene
        }
        else
        {
            Destroy(gameObject);
        }

        // Crea due AudioSources per la musica e gli effetti sonori
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        soundEffectsAudioSource = gameObject.AddComponent<AudioSource>();

        // Imposta i volumi in base alle preferenze salvate
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        soundEffectsAudioSource.volume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);

        
    }

    private void Start()
    {
        slider.GetComponent<Slider>().value = 0.8f;
        SetMusicVolume(0.8f);
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
        MUSIC_MIXER.audioMixer.SetFloat("MUSIC_MASTER", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        //Debug.Log(volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsAudioSource.volume = volume;
        EFX_MIXER.audioMixer.SetFloat("EFX_MASTER", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SoundEffectsVolume", Mathf.Log10(volume) * 20);
        //Debug.Log("Effetti");
    }

    public void PlayMusic(AudioClip musicClip)
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        soundEffectsAudioSource.PlayOneShot(soundEffectClip);
    }
}

// made with love from Assassin's script ♥