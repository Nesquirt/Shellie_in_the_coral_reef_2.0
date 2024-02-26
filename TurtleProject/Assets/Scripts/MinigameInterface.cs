using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameInterface : MonoBehaviour
{
    private GameObject minigamePanel;
    private TextMeshProUGUI timer, scoreText;
    private Image minigameIcon, keyIcon;

    [Header("--- Sprites ---")]
    public Sprite obstacleCourseSprite;
    public Sprite trashCollectingSprite;
    public Sprite mazeExploringSprite;
    public Sprite keySprite;
    private void Awake()
    {
        minigamePanel = GameObject.Find("Canvas/MinigamePanel");
        minigamePanel.SetActive(false);

        timer = minigamePanel.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        scoreText = minigamePanel.transform.Find("ScoreText").gameObject.GetComponent<TextMeshProUGUI>();

        minigameIcon = minigamePanel.transform.Find("MinigameIcon").gameObject.GetComponent<Image>();
        keyIcon = minigamePanel.transform.Find("KeyIcon").gameObject.GetComponent<Image>();
        keyIcon.sprite = keySprite;

    }

    public void startMinigame()
    {
        minigamePanel.SetActive(true);
        timer.SetText("0:00:00");
        
        switch(GameDirector.Instance.getGameState())
        {
            case GameDirector.GameState.ObstacleCourse:
                minigameIcon.sprite = obstacleCourseSprite;

                break;

            case GameDirector.GameState.TrashCollecting:
                minigameIcon.sprite = trashCollectingSprite;

                break;

            case GameDirector.GameState.MazeExploring:
                minigameIcon.sprite = mazeExploringSprite;

                break;


        }
    }

    public void endMinigame()
    {
        minigamePanel.SetActive(false);
    }

    //Imposta il testo del timer; chiamato nelle funzioni dei minigiochi.
    //Nota: il parametro time è attualmente in decimi di secondo.
    public void setTimerText(int time)
    {
        timer.SetText(TimeToString(time));
    }

    //Metodo che ritorna una stringa in formato x:xx:xx;
    //prende in input un numero di decimi di secondo.
    private string TimeToString(int currentTenths)
    {
        int minutes = currentTenths / 600;
        int seconds = (currentTenths % 600) / 10;
        int tenths = currentTenths % 10;

        if (seconds < 10)
            return minutes + ":0" + seconds + ":" + tenths + "0";
        else
            return minutes + ":" + seconds + ":" + tenths + "0";

    }
    public void setScoreText(int current, int max)
    {
        scoreText.SetText(current + "/" + max);
    }

    public void toggleKeyIcon()
    {
        if (keyIcon.IsActive())
        {
            keyIcon.gameObject.SetActive(false);
        }
        else keyIcon.gameObject.SetActive(true);
    }

}
