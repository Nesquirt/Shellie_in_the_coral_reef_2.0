using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VictoryInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardsText;
    private void Awake()
    {
        this.toggleVictoryPanel(false);
    }

    public void setRewardsText(string text)
    {
        rewardsText.SetText(text);
    }
    public void toggleVictoryPanel(bool state)
    {
        this.gameObject.SetActive(state);
    }

}
