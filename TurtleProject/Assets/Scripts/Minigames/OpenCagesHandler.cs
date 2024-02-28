
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCagesHandler : MonoBehaviour
{
    private float timeRemaining;
    private float seconds;

    private bool hasKey;
    private int openCages;
    private int totCages;
    private int finalFunctionCalled;

    private GameObject[] arr_cages;
    [SerializeField] private UIManager UImanager;
    private AudioManager audioManager;

    void Awake()
    {
        this.totCages = this.GetComponent<SpawnCages>().totalCages;  //prendo il numero di casse
    }

    //Funzione START MazeExploring
    public void restartMazeGame()
    {
        if (UImanager.dialogueInterface.getCurrentNPC() == "Dory")
        {
            GameDirector.Instance.setGameState(GameDirector.GameState.MazeExploring);
            this.finalFunctionCalled = 0;
            this.timeRemaining = 180f;
            this.seconds = Mathf.Round(timeRemaining);
            Debug.Log("SECONDI: " + this.seconds);

            this.hasKey = false;
            this.openCages = 0;

            UImanager.minigameInterface.setScoreText(0, totCages);
            this.GetComponent<SpawnCages>().restartGame();

            GameDirector.Instance.audioManager.MiniGame();
        }
    }

    private void FixedUpdate()
    {

        if (GameDirector.Instance.getGameState() != GameDirector.GameState.MazeExploring)
            return;


        if (this.timeRemaining > 0)
        {
            this.timeRemaining -= Time.deltaTime;
            this.seconds = Mathf.Round(timeRemaining);
            UImanager.minigameInterface.setTimerText((int)seconds);
        }
        else
        {
            if (finalFunctionCalled == 0)
            {
                callFinalFunction();
            }
            finalFunctionCalled++;
        }
    }
    public void callFinalFunction()
    {
        checkEnd();
        GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
        this.hasKey = false;

        //faccio scomparire le chiavi
        GameObject[] arr_keys = GameObject.FindGameObjectsWithTag("Chiave");
        for (int i = 0; i < arr_keys.Length; i++)
        {
            Destroy(arr_keys[i]);
        }

        //riempie array di gabbie
        this.arr_cages = GameObject.FindGameObjectsWithTag("Gabbia");
        Debug.Log("dim array: " + arr_cages.Length);

        if (arr_cages.Length != 0)
        {
            Debug.Log("arr_cages non NULL");

            for (int i = 0; i < totCages; i++)
            {
                if (arr_cages[i] != null)
                {
                    arr_cages[i].GetComponent<CageScript>().GoUp();
                }

            }
            Debug.Log("arr_cages.Length: " + arr_cages.Length);
        }
        else
            GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);

    }

    private void checkEnd()
    {
        int gainedPearls = 8 * openCages;
        if (openCages == 0)
        {
            UImanager.victoryInterface.setRewardsText("Purtroppo non sei riuscita a liberare nesssun granchio... \n" +
                                    "Non hai guadagnato nessuna perla...\n" +
                                    "Il livello di biodiversità NON � aumentato...");
        }
        else if (openCages == totCages)
        {
            UImanager.victoryInterface.setRewardsText("Complimenti! Sei riuscita a salvare tutti i granchi! \n" +
                                "Perle guadagnate: " + gainedPearls + "\n" +
                                "Livello di biodiversità aumentato del " + gainedPearls + "%");
        }
        else
        {
            UImanager.victoryInterface.setRewardsText("Perle guadagnate: " + gainedPearls + "\n" +
                                "Livello di biodiversità aumentato del " + gainedPearls + "%");
        }
        UImanager.victoryInterface.toggleVictoryPanel(true);

        GameDirector.Instance.addPearls(gainedPearls);
        GameDirector.Instance.addParameters(0, 0, gainedPearls);

        GameDirector.Instance.audioManager.MiniGame();
    }

    //metodo chiamato da TurtleController
    public void TriggerMethod(Collider other)
    {
        if (other.CompareTag("Chiave"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!hasKey)
                {
                    Destroy(other.gameObject);
                    this.hasKey = true;
                    UImanager.minigameInterface.toggleKeyIcon(true);
                    Debug.Log("ho la chiave");
                    GameDirector.Instance.audioManager.KeyOrCage(other);
                }
                else
                    return;
            }
        }

        if (other.CompareTag("Gabbia"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (hasKey && other.GetComponent<CageScript>().isLocked)
                {
                    Debug.Log("apro gabbia");
                    other.GetComponent<CageScript>().OpenCage();
                    this.hasKey = false;
                    UImanager.minigameInterface.toggleKeyIcon(false);
                    this.openCages++;
                    Debug.Log("GABBIE APERTE: " + this.openCages);
                    GameDirector.Instance.audioManager.KeyOrCage(other);
                    UImanager.minigameInterface.setScoreText(openCages, totCages);
                    if (openCages == totCages && finalFunctionCalled == 0)
                    {
                        callFinalFunction();
                        finalFunctionCalled++;
                    }
                }
                else
                    return;

            }
        }
    }

}