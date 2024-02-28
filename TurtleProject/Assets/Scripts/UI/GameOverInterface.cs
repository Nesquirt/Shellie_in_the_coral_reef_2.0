using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText, centralText, bottomText;
    [SerializeField] private GameObject websiteButton, returnToMenuButton;

    private void Awake()
    {
        titleText.gameObject.SetActive(false);
        centralText.gameObject.SetActive(false);
        bottomText.gameObject.SetActive(false);
        websiteButton.SetActive(false);
        returnToMenuButton.SetActive(false);
    }
    public void toggleGameOverPanel(bool state)
    {
        this.gameObject.SetActive(state);
    }
    public void GameOver(bool win)
    {
        if (!win)
        {
            titleText.SetText("GAME OVER!");
            centralText.SetText("Le condizioni dell'ambiente si sono deteriorate, e i coralli stanno iniziando a perdere colore... " +
                "Dovremo riprovare da un'altra parte.");
        }
        else
        {
            titleText.SetText("VITTORIA!");
            centralText.SetText("Grazie ai tuoi sforzi, le condizioni dell'ambiente sono stabili e in grado di prosperare. " +
                "Questa barriera corallina e' salva!");
        }

        toggleGameOverPanel(true);
        StartCoroutine(FadeIn(transform.GetComponent<Image>()));
    }
    // -------------------------------------------------------------------- //
    //Listener per i bottoni della schermata finale (apri sito web e torna al menu')
    public void OpenURL()
    {
        GameDirector.Instance.audioManager.ButtonPressed();
        Application.OpenURL("https://coralreefrescueinitiative.org/");
    }
    public void LoadMenu()
    {
        CancelInvoke();
        GameDirector.Instance.audioManager.ButtonPressed();

        SceneManager.LoadScene("Simone_Menu_Iniziale");
    }

    // -------------------------------------------------------------------- //
    //Coroutine per la comparsa del pannello e delle scritte
    IEnumerator FadeIn(Image GameOverPanel)
    {
        float fadeAmount;
        Color nextFrameColor;
        while (GameOverPanel.color.a < 1)
        {
            fadeAmount = GameOverPanel.color.a + (Time.deltaTime);
            nextFrameColor = new Color(GameOverPanel.color.r, GameOverPanel.color.g, GameOverPanel.color.b, fadeAmount);
            GameOverPanel.color = nextFrameColor;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);

        titleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        centralText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        bottomText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        websiteButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        returnToMenuButton.gameObject.SetActive(true);


    }
}
