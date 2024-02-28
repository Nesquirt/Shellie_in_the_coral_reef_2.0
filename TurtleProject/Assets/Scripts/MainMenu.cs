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
        }

        LoadingPanel = GameObject.Find("Canvas/LoadingPanel");
        LoadingPanel.SetActive(false);
        GameDirector.Instance.audioManager.MenuMusic();
    }

    // Funzione per avviare una nuova partita
    public void StartGame()
    {
        LoadingPanel.SetActive(true);
        GameDirector.Instance.audioManager.ButtonPressed();
        GameDirector.Instance.audioManager.GameMusic();
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
        GameDirector.Instance.audioManager.ButtonPressed();
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
        GameDirector.Instance.audioManager.ButtonPressed();
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
            SceneManager.LoadScene("InterfaceScene");
    }
}


// made with love from Assassin's script ♥