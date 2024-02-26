using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VictoryInterface : MonoBehaviour
{
    private static GameObject victoryPanel;
    private static TextMeshProUGUI rewardsText;

    private void Awake()
    {
        victoryPanel = GameObject.Find("Canvas/VictoryPanel");
        victoryPanel.SetActive(false);
        rewardsText = victoryPanel.transform.Find("RewardsPanel/RewardsText").GetComponent<TextMeshProUGUI>();
    }

    public static void setRewardsText(string text)
    {
        rewardsText.SetText(text);
    }
    public static void toggleVictoryPanelOn()
    {
        victoryPanel.SetActive(true);
    }

}
