using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button optionsButton;
    public Button quitButton;
    public Button aboutButton;

    void Start()
    {
        /* Associa le funzioni ai pulsanti
        startGameButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        aboutButton.onClick.AddListener(GameDirector.Instance.OpenURL); */
    }

    // Funzione per avviare una nuova partita
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Funzione per aprire il menu delle impostazioni (la scena SettingsMenu)
    public void OpenOptions()
    {
        if ("Simone_impostazioni" == CheckIfSceneLoaded())
        {
            Debug.Log("La scena è già caricata");
            return;
        }
        SceneManager.LoadScene("Simone_impostazioni", LoadSceneMode.Additive);
    }
    public string CheckIfSceneLoaded()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        return currentScene.name;
    }
    // Funzione per chiudere il gioco
    public void QuitGame()
    {
        Application.Quit();
    }

}


// made with love from Assassin's script ♥