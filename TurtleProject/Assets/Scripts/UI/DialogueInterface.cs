using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// -------------------------------------------------------------------------- //
//Questo script si occupa sia del prompt di dialogo (premi E per parlare con...)
//che del pannello di avvio del minigioco.
// -------------------------------------------------------------------------- //

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
