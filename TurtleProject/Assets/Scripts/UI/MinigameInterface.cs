using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameInterface : MonoBehaviour
{
    private static GameObject minigamePanel;
    private static TextMeshProUGUI timer, scoreText;
    private static Image minigameIcon, keyIcon;
    private static Sprite obstacleCourseSprite, trashCollectingSprite, mazeExploringSprite, keySprite;

    private void Awake()
    {
        minigamePanel = GameObject.Find("Canvas/MinigamePanel");
        Debug.Log(minigamePanel.name);
        minigamePanel.SetActive(false);

        timer = minigamePanel.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        scoreText = minigamePanel.transform.Find("ScoreText").gameObject.GetComponent<TextMeshProUGUI>();

        minigameIcon = minigamePanel.transform.Find("MinigameIcon").gameObject.GetComponent<Image>();
        keyIcon = minigamePanel.transform.Find("KeyIcon").gameObject.GetComponent<Image>();

        obstacleCourseSprite = Resources.Load("Sprites/ObstacleCourseSprite") as Sprite;
        trashCollectingSprite = Resources.Load("Sprites/TrashCollectingSprite") as Sprite;
        mazeExploringSprite = Resources.Load("Sprites/MazeExploringSprite") as Sprite;
        keySprite = Resources.Load("Sprites/KeySprite") as Sprite;
        keyIcon.sprite = keySprite;

    }

    public static void startMinigame()
    {
        minigamePanel.SetActive(true);
        timer.SetText("0:00:00");
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

    public static void endMinigame()
    {
            minigamePanel.SetActive(false);
    }

    //Imposta il testo del timer; chiamato nelle funzioni dei minigiochi.
    //Nota: il parametro time è attualmente in decimi di secondo.
    public static void setTimerText(int time)
    {
        timer.SetText(TimeToString(time));
    }

    //Metodo che ritorna una stringa in formato x:xx:xx;
    //prende in input un numero di decimi di secondo.
    private  static string TimeToString(int currentTime)
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
    public static void setScoreText(int current, int max)
    {
        scoreText.SetText(current + "/" + max);
    }

    public static void toggleKeyIcon(bool state)
    { 
        keyIcon.gameObject.SetActive(state);
    }
}
