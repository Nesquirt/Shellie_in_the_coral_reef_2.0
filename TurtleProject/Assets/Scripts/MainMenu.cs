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
        // Associa le funzioni ai pulsanti
        startGameButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        aboutButton.onClick.AddListener(GameDirector.Instance.OpenURL);
    }

    // Funzione per avviare una nuova partita
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Funzione per aprire il menu delle impostazioni (la scena SettingsMenu)
    public void OpenOptions()
    {
        StartCoroutine(LoadOptions());
    }

    // Funzione per chiudere il gioco
    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator LoadOptions()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene("Simone_impostazioni", LoadSceneMode.Additive);
        yield return new WaitForEndOfFrame();
    }
   
}


// made with love from Assassin's script ♥