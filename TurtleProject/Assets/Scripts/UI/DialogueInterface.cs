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
    private static GameObject dialoguePanel;
    private static TextMeshProUGUI NPCName, dialogueText;
    private static string currentNPC;

    private void Awake()
    {
        dialoguePanel = GameObject.Find("Canvas/DialoguePanel");
        dialoguePanel.SetActive(false);

        NPCName = dialoguePanel.transform.Find("TitlePanel/NPCName").GetComponent<TextMeshProUGUI>();
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();

    }

    public static void setDialogueText(string text)
    {
        dialogueText.SetText(text);
    }
    public static void setNPCName(string name)
    {
        NPCName.SetText(name);
    }

    public static void toggleDialoguePanelOn()
    {
        dialoguePanel.SetActive(true);
    }
    public static void toggleDialoguePanelOff()
    {
        dialoguePanel.SetActive(false);
    }
    public static void setCurrentNPC(string NPC)
    {
        currentNPC = NPC;
    }
    public static string getCurrentNPC()
    {
        return currentNPC;
    }
    // -------------------------------------------------------------------- //
    //Listener dei bottoni di dialogo
    public void ConfirmButton_onClick()
    {
        toggleDialoguePanelOff();
        switch(currentNPC)
        {
            case "Anguilla":
                GameDirector.Instance.setGameState(GameDirector.GameState.ObstacleCourse);
                break;

        }
    }

    public void CancelButton_onClick()
    {
        toggleDialoguePanelOff();
        PromptInterface.togglePromptOn();
    }
    /*
    public void ConfirmButton_onClick()
    {
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        obstacleRacePrompt.gameObject.SetActive(false);
        canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
        raceStart();
        audioManager.PlaySFX(audioManager.selection);
        audioManager.ChangeMusic(audioManager.MatteoGameSountrack, true, 0.12f);
    }
    public void CancelButton_onClick()
    {
        AnguillaTriggerExit();
        obstacleRacePrompt.gameObject.SetActive(true);
        audioManager.PlaySFX(audioManager.selection);
    }
    */




}
