
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCagesHandler : MonoBehaviour
{
    private GameObject canvas;
    private TextMeshProUGUI timer_text, crab_text, NPCName, dialogueText, victoryText;
    private Transform MazePrompt;
    private Image crab_icon, key_icon;
    private Button confirmButton, cancelButton;

    private float timeRemaining;
    private float seconds;

    private bool hasKey;
    private int openCages;
    private int totCages;

    private GameObject[] arr_cages;

    void Awake()
    {
        this.totCages = this.GetComponent<SpawnCages>().totalCages;  //prendo il numero di casse
        this.canvas = GameObject.Find("Canvas");

        this.timer_text = canvas.transform.Find("MazeContainer/TimerText").gameObject.GetComponent<TextMeshProUGUI>();
        this.crab_icon = canvas.transform.Find("MazeContainer/CrabIcon").gameObject.GetComponent<Image>();
        this.key_icon = canvas.transform.Find("MazeContainer/KeyIcon").gameObject.GetComponent<Image>();
        this.crab_text = canvas.transform.Find("MazeContainer/FreedCrab").gameObject.GetComponent<TextMeshProUGUI>();

        //Elementi finestra di dialogo
        this.MazePrompt = canvas.transform.Find("MazeContainer/MazePrompt");
        MazePrompt.gameObject.SetActive(false);
        this.NPCName = canvas.transform.Find("DialoguePanel/TitlePanel/NPCName").gameObject.GetComponent<TextMeshProUGUI>();
        this.dialogueText = canvas.transform.Find("DialoguePanel/DialogueText").gameObject.GetComponent<TextMeshProUGUI>();
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);

        //Bottoni 
        confirmButton = canvas.transform.Find("DialoguePanel/ConfirmButton").gameObject.GetComponent<Button>();
        cancelButton = canvas.transform.Find("DialoguePanel/CancelButton").gameObject.GetComponent<Button>();

        timer_text.enabled = false;
        crab_icon.enabled = false;
        key_icon.enabled = false;
        crab_text.enabled = false;
    }

    //Funzione START MazeExploring
    public void restartMazeGame()
    {
        this.timeRemaining = 120f;
        this.seconds = Mathf.Round(timeRemaining);
        Debug.Log("SECONDI: " + this.seconds);

        this.hasKey = false;
        this.openCages = 0;

        timer_text.enabled = true;
        timer_text.SetText(seconds.ToString());

        crab_icon.enabled = true;
        key_icon.enabled = false;

        crab_text.enabled = true;
        crab_text.SetText(openCages.ToString() + "/" + totCages.ToString());

        this.GetComponent<SpawnCages>().restartGame();
    }

    //chiamato quando Shelly si avvicina al pesce
    public void mazeStartPrompt()
    {
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
            return;

        if (!canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
        {
            MazePrompt.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("BarsPanel").gameObject.SetActive(false);
            MazePrompt.gameObject.SetActive(false);
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(true);
            NPCName.SetText("Dory");
            dialogueText.SetText("Hey Shelly! Ci sono dei granchi che hanno bisogno di essere liberati! \n" +
                "Ti va di aiutarmi?" + " Nel labirinto troverai delle chiavi con cui poter aprire le gabbie \n" +
                "Attenta! Puoi prendere solo una chiave alla volta ed hai 3 minuti di tempo per liberarli tutti \n");
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmMazeButton_onClick);
            cancelButton.onClick.AddListener(CancelMazeButton_onClick);
        }
    }

    public void PesceTriggerExit()
    {
        if (canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);

        if (MazePrompt.gameObject.activeSelf)
            MazePrompt.gameObject.SetActive(false);

        if (!canvas.transform.Find("BarsPanel").gameObject.activeSelf)
            canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
    }

    //Listener bottoni di dialogo
    public void ConfirmMazeButton_onClick()
    {
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        MazePrompt.gameObject.SetActive(false);
        canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
        GameDirector.Instance.setGameState(GameDirector.GameState.MazeExploring);
        restartMazeGame();
    }
    public void CancelMazeButton_onClick()
    {
        PesceTriggerExit();
        MazePrompt.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (GameObject.Find("Director").GetComponent<GameDirector>().getGameState() != GameDirector.GameState.MazeExploring)
            return;

        //Debug.Log("UPDATE");



        if (this.timeRemaining > 0)
        {
            this.timeRemaining -= Time.deltaTime;
            this.seconds = Mathf.Round(timeRemaining);
            this.timer_text.SetText(seconds.ToString());
        }


        if (IsFinished())
        {
            checkEnd();

            this.timer_text.enabled = false;
            this.crab_text.enabled = false;
            this.crab_icon.enabled = false;
            this.key_icon.enabled = false;
            this.hasKey = false;

            //faccio scomparire le chiavi
            GameObject[] arr_keys = GameObject.FindGameObjectsWithTag("Chiave");
            for (int i = 0; i < arr_keys.Length; i++)
            {
                Destroy(arr_keys[i]);
            }

            //ANIMAZIONE gabbie che salgono 
            this.arr_cages = GameObject.FindGameObjectsWithTag("Gabbia");
            Debug.Log("dim array: " + arr_cages.Length);

            if (arr_cages.Length != 0)
            {
                //Debug.Log(arr_cages);
                //StartCoroutine(cageGoesUp());
                for (int i = 0; i < arr_cages.Length; i++)
                {
                    if (arr_cages[i] != null)
                    {
                        arr_cages[i].GetComponent<CageScript>().GoUp();
                        if (arr_cages[i].transform.position.y > 100f)
                        {
                            //arr_cages[i].GetComponent<Rigidbody>().isKinematic = true;      //ho già disabilitato la fisica per l'oggetto
                            Debug.Log("gabbia distrutta");
                            Destroy(arr_cages[i]);
                        }
                    }

                }
            }
            else
            {
                GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);
            }

        }
    }

    private void checkEnd()
    {
        int gane = 8 * openCages;

        canvas.transform.Find("VictoryPanel").gameObject.SetActive(true);
        victoryText = canvas.transform.Find("VictoryPanel/RewardsPanel/RewardsText").GetComponent<TextMeshProUGUI>();

        if (openCages == 0)
        {
            victoryText.SetText("Purtroppo non sei riuscita a liberare nesssun granchio... \n" +
                                "Non hai guadagnato nessuna perla...\n" +
                                "Il livello di ossigeno NON è aumentato...");
        }
        else if (openCages == totCages)
        {
            victoryText.SetText("Complimenti! Sei riuscita a salvare tutti i granchi! \n" +
                                "Perle guadagnate: " + gane + "\n" +
                                 "Livello di ossigeno aumentato del " + gane + "%");
        }
        else
        {
            victoryText.SetText("Perle guadagnate: " + gane + "\n" +
                            "Livello di ossigeno aumentato del " + gane + "%");
        }

        GameDirector.Instance.addPearls(gane);
        GameDirector.Instance.addBiodiversity(gane);
    }


    //TODO: sistemare coroutine
    IEnumerator cageGoesUp()
    {

        for (int i = 0; i < arr_cages.Length; i++)
        {
            if (arr_cages[i] != null)
            {
                arr_cages[i].GetComponent<CageScript>().GoUp();
                if (arr_cages[i].transform.position.y > 120f)
                {
                    //arr_cages[i].GetComponent<Rigidbody>().isKinematic = true;      //ho già disabilitato la fisica per l'oggetto
                    Debug.Log("gabbia distrutta");
                    Destroy(arr_cages[i]);
                }
            }

        }
        yield return new WaitForSeconds(4);
        this.gameObject.SetActive(false);
        StopCoroutine(cageGoesUp());

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
                    this.key_icon.enabled = true;
                    Debug.Log("ho la chiave");
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
                    this.key_icon.enabled = false;
                    this.openCages++;
                    Debug.Log("GABBIE APERTE: " + this.openCages);
                    this.crab_text.SetText(openCages.ToString() + "/" + totCages.ToString());
                }
                else
                    return;

            }
        }
    }
    private bool IsFinished()
    {
        if (this.openCages == this.totCages || this.seconds == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}