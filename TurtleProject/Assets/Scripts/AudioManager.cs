using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicAudioSource;
    private AudioSource soundEffectsAudioSource;

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

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Debug.Log(volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
        Debug.Log("Effetti");
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