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

    private void Awake()
    {
        minigamePanel = GameObject.Find("Canvas/MinigamePanel");
        minigamePanel.SetActive(false);

        timer = minigamePanel.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        scoreText = minigamePanel.transform.Find("ScoreText").gameObject.GetComponent<TextMeshProUGUI>();

        minigameIcon = minigamePanel.transform.Find("MinigameIcon").gameObject.GetComponent<Image>();
        keyIcon = minigamePanel.transform.Find("KeyIcon").gameObject.GetComponent<Image>();

    }

    public void startMinigame()
    {
        minigamePanel.SetActive(true);
        timer.SetText("0:00:00");

    }

    public void endMinigame()
    {
        minigamePanel.SetActive(false);
    }

}
