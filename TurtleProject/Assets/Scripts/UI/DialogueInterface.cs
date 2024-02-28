using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueInterface : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI NPCName, dialogueText;
    [SerializeField] private string currentNPC;

    private void Awake()
    {
        toggleDialoguePanel(false);
    }

    public void setDialogueText(string text)
    {
        dialogueText.SetText(text);
    }
    public void setNPCName(string name)
    {
        NPCName.SetText(name);
    }
    public void toggleDialoguePanel(bool state)
    {
        dialoguePanel.SetActive(state);
    }
    public void setCurrentNPC(string NPC)
    {
        currentNPC = NPC;
    }
    public string getCurrentNPC()
    {
        return currentNPC;
    }

}
