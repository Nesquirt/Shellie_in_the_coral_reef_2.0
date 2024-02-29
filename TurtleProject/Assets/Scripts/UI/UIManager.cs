using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public BarsInterface barsInterface;
    public CoralChoiceInterface coralChoiceInterface;
    public DialogueInterface dialogueInterface;
    public GameOverInterface gameOverInterface;
    public MinigameInterface minigameInterface;
    public PromptInterface promptInterface;
    public VictoryInterface victoryInterface;

    private GameObject[] corals;

    private void Awake()
    {
        corals = GameObject.FindGameObjectsWithTag("CoralSpot");
        /*
        // -------------------------------------------------------------------- //
        //Controlla tutti gli oggetti con tag "Corallo", e li mette nell'array

        if (corals.Length != 0)
            Array.Clear(corals, 0, corals.Length);

        corals = GameObject.FindGameObjectsWithTag("CoralSpot");
        */
    }

    public void toggleOutline(bool state)
    {
        foreach (GameObject coralSpot in corals)
        {
            coralSpot.GetComponent<CoralHandler>().toggleOutline(state);
        }
    }
}
