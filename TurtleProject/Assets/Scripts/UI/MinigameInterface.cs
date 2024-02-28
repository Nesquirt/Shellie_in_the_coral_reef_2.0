using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameInterface : MonoBehaviour
{
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private TextMeshProUGUI timer, scoreText;
    [SerializeField] private Image minigameIcon, keyIcon;
    [SerializeField] private Sprite obstacleCourseSprite, trashCollectingSprite, mazeExploringSprite, keySprite;

    private void Awake()
    {
        minigamePanel = GameObject.Find("Canvas/MinigamePanel");
        Debug.Log(minigamePanel.name);
        minigamePanel.SetActive(false);

        timer = minigamePanel.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        scoreText = minigamePanel.transform.Find("ScoreText").gameObject.GetComponent<TextMeshProUGUI>();

        minigameIcon = minigamePanel.transform.Find("MinigameIcon").gameObject.GetComponent<Image>();
        keyIcon = minigamePanel.transform.Find("KeyIcon").gameObject.GetComponent<Image>();

        obstacleCourseSprite = Resources.Load<Sprite>("Sprites/ObstacleCourseSprite");
        trashCollectingSprite = Resources.Load<Sprite>("Sprites/TrashCollectingSprite");
        mazeExploringSprite = Resources.Load<Sprite>("Sprites/MazeExploringSprite");
        keySprite = Resources.Load<Sprite>("Sprites/KeySprite");
        keyIcon.sprite = keySprite;

    }

    public void startMinigame()
    {
        minigamePanel.SetActive(true);
        toggleKeyIcon(false);
        switch(GameDirector.Instance.getGameState())
        {
            case GameDirector.GameState.ObstacleCourse:
                minigameIcon.sprite = obstacleCourseSprite;
                setScoreText(0, 29);
                break;

            case GameDirector.GameState.TrashCollecting:
                minigameIcon.sprite = trashCollectingSprite;
                setScoreText(0, 5);
                break;

            case GameDirector.GameState.MazeExploring:
                minigameIcon.sprite = mazeExploringSprite;
                setScoreText(0, 4);
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
    private string TimeToString(int currentTime)
    {
        int minutes, seconds, tenths;
        switch(GameDirector.Instance.getGameState())
        {
            case GameDirector.GameState.ObstacleCourse:
                minutes = currentTime / 600;
                seconds = (currentTime % 600) / 10;
                tenths = currentTime % 10;

                if (seconds < 10)
                    return minutes + ":0" + seconds + ":" + tenths + "0";
                else
                    return minutes + ":" + seconds + ":" + tenths + "0";

            case GameDirector.GameState.MazeExploring:
            case GameDirector.GameState.TrashCollecting:
                minutes = currentTime / 60;
                seconds = currentTime % 60;

                if (seconds < 10)
                    return minutes + ":0" + seconds;
                else
                    return minutes + ":" + seconds;
            default:
                return "0:00:00";

        }
    }
    public void setScoreText(int current, int max)
    {
        scoreText.SetText(current + "/" + max);
    }

    public void toggleKeyIcon(bool state)
    { 
        keyIcon.gameObject.SetActive(state);
    }
}
