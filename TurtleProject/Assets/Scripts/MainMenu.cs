using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button optionsButton;
    public Button quitButton;
    public Button aboutButton;

    public GameObject optionsPanel;

    void Start()
    {
        // Associa le funzioni ai pulsanti
        startGameButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        aboutButton.onClick.AddListener(OpenAbout);

        // Nascondi il pannello delle opzioni all'avvio
        optionsPanel.SetActive(false);
    }

    // Funzione per avviare una nuova partita
    public void StartGame()
    {
        SceneManager.LoadScene("Matteo_Terrain"); 
    }

    // Funzione per aprire le opzioni
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    // Funzione per chiudere il pannello delle opzioni
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // Funzione per chiudere il gioco
    public void QuitGame()
    {
        Application.Quit();
    }

    // Funzione per aprire il link "About" nel browser
    public void OpenAbout()
    {
        Application.OpenURL("https://coralreefrescueinitiative.org/");
    }
}
