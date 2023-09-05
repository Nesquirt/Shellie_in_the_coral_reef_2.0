using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;
    public Button backButton;

    private void Start()
    {
        // Slider
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);

        // Aggiungi i listener per gli eventi dei pulsanti
        backButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDestroy()
    {
        // Rimuovi i listener per gli eventi dei pulsanti per evitare memory leak
        backButton.onClick.RemoveListener(BackToMainMenu);
    }

    public void BackToMainMenu()
    {
        // Carica la scena del menu principale
        SceneManager.LoadScene("Simone_Menu_iniziale");
    }

    public void SetMusicVolume(float volume)
    {
        // Imposta il volume della musica e salva il valore nelle preferenze
        AudioManager.Instance.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        // Imposta il volume degli effetti sonori e salva il valore nelle preferenze
        AudioManager.Instance.SetSoundEffectsVolume(volume);
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);
    }
}

// made with love from Assassin's script ♥