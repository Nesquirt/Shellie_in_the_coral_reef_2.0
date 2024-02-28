using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider MusicVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    [SerializeField] private AudioMixer myMixer;
    public Button backButton;
    private AudioManager audioManager;

    private void Start()
    {
        //Slider start values
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else 
        {
            SetMusicVolume();
            SetSoundEffectsVolume();
        }

        // Aggiungi i listener per gli eventi dei pulsanti
        backButton.onClick.AddListener(BackToMainMenu);
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnDestroy()
    {
        // Rimuovi i listener per gli eventi dei pulsanti per evitare memory leak
        backButton.onClick.RemoveListener(BackToMainMenu);
    }

    public void BackToMainMenu()
    {
        // Carica la scena del menu principale
        audioManager.ButtonPressed();
        SceneManager.UnloadSceneAsync("Simone_impostazioni");
    }
    public void LoadVolume()
    {
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume();
        SetSoundEffectsVolume();
    }
    public void SetMusicVolume()
    {
        // Imposta il volume della musica nel mixer
        float volume = MusicVolumeSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSoundEffectsVolume()
    {
        // Imposta il volume degli SFX nel mixer
        float volume = SFXVolumeSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}

// made with love from Assassin's script ♥