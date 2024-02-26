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

    public Sprite obstacleCourseSprite, trashCollectingSprite, mazeExploringSprite, keySprite;

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

    public void setTimerText(string text)
    {
        timer.SetText(text);
    }

}
