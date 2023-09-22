using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button optionsButton;
    public Button quitButton;
    public Button aboutButton;

    public GameObject LoadingPanel;
    void Start()
    {
        if (GameDirector.Instance == null)
        {
            //Non deve fare nulla; questo if è chiamato solo per inizializzare il director nella scena
        }

        /* Associa le funzioni ai pulsanti
        startGameButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        aboutButton.onClick.AddListener(GameDirector.Instance.OpenURL); */
        LoadingPanel = GameObject.Find("Canvas/LoadingPanel");
        LoadingPanel.SetActive(false);
    }

    // Funzione per avviare una nuova partita
    public void StartGame()
    {
        LoadingPanel.SetActive(true);
        StartCoroutine(FadeInLoadingScreen());
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
    IEnumerator FadeInLoadingScreen()
    {
        Image LoadingPanelImage = LoadingPanel.GetComponent<Image>();
        TextMeshProUGUI LoadingTextImage = LoadingPanel.transform.Find("LoadingText").GetComponent<TextMeshProUGUI>();
        float fadeAmount;
        Color nextPanelColor, nextTextColor;
        while (LoadingPanelImage.color.a < 1)
        {
            fadeAmount = LoadingPanelImage.color.a + (Time.deltaTime*5);
            nextPanelColor = new Color(LoadingPanelImage.color.r, LoadingPanelImage.color.g, LoadingPanelImage.color.b, fadeAmount);
            nextTextColor = new Color(LoadingTextImage.color.r, LoadingTextImage.color.g, LoadingTextImage.color.b, fadeAmount);
            LoadingPanelImage.color = nextPanelColor;
            LoadingTextImage.color = nextTextColor;
            yield return new WaitForSeconds(0.01f);
        }
            SceneManager.LoadScene("GameScene");
    }
}


// made with love from Assassin's script ♥