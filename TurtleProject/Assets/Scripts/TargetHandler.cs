using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetHandler : MonoBehaviour
{
    private GameObject player;
    private GameObject directorObject;
    private GameObject canvas;
    private GameDirector director;
    //private GameObject[] targets;
    [SerializeField] private Material activeMaterial, inactiveMaterial;
    private int currentTime;        //Tempo attuale del timer in millisecondi

    private int currentTenths;      //Numero di ostacoli superati dal giocatore
    public int targetNumber;        //L'indice di questo ostacolo, per sapere quando deve diventare attivo

    private Rigidbody rb;

    private Transform obstacleRacePrompt;
    private TextMeshProUGUI NPCName, dialogueText, rewardsText, timer;
    private Button confirmButton, cancelButton;
    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        obstacleRacePrompt = canvas.transform.Find("ObstacleRacePrompt");
        obstacleRacePrompt.gameObject.SetActive(false);
        NPCName = canvas.transform.Find("DialoguePanel/TitlePanel/NPCName").gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = canvas.transform.Find("DialoguePanel/DialogueText").gameObject.GetComponent<TextMeshProUGUI>();
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        confirmButton = canvas.transform.Find("DialoguePanel/ConfirmButton").gameObject.GetComponent<Button>();
        cancelButton = canvas.transform.Find("DialoguePanel/CancelButton").gameObject.GetComponent<Button>();
        timer = canvas.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        confirmButton.onClick.AddListener(ConfirmButton_onClick);
        cancelButton.onClick.AddListener(CancelButton_onClick);

        player = GameObject.Find("TurtlePlayer");
        directorObject = GameObject.Find("Director");
        director = directorObject.GetComponent<GameDirector>();
        

        //raceStart(); //TODO: rimuovi e mettilo come prompt
    }
    // -------------------------------------------------------------------- //
    //Funzioni chiamate all'interno di TurtleController per gestire il dialogo con l'anguilla
    public void raceStartPrompt()
    {
        if (director.getGameState() != GameDirector.GameState.FreeRoaming)
            return;
        if(!canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            obstacleRacePrompt.gameObject.SetActive(true);
        if(Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("BarsPanel").gameObject.SetActive(false);
            obstacleRacePrompt.gameObject.SetActive(false);
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(true);
            NPCName.SetText("Anguilla");
            dialogueText.SetText("Hey, tu! Sembri una tipa molto in forma. Ti andrebbe di aiutarmi con una faccenda?\n" +
                                 "Le alghe in questo canyon sono in acqua stagnante... Svegliale attraversando tutti gli anelli rocciosi!\n" +
                                 "Sono sicura che il livello di ossigeno ne aumenterà... E se vai abbastanza veloce, ti darò anche qualche perla in più. Ci stai?");
        }
    }
    public void AnguillaTriggerExit()
    {
        if (canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        if(obstacleRacePrompt.gameObject.activeSelf)
            obstacleRacePrompt.gameObject.SetActive(false);
        if(!canvas.transform.Find("BarsPanel").gameObject.activeSelf)
            canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
    }
    public void ConfirmButton_onClick()
    {
        raceStart();
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        obstacleRacePrompt.gameObject.SetActive(false);
        canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
    }
    public void CancelButton_onClick()
    {
        AnguillaTriggerExit();
        obstacleRacePrompt.gameObject.SetActive(true);
    }
    // -------------------------------------------------------------------- //
    //Funzione di start del minigioco
    public void raceStart()
    {
        // -------------------------------------------------------------------- //
        //GAMESTATE
        //Check per vedere se un altro minigioco è attivo
        if (director.getGameState() != GameDirector.GameState.FreeRoaming)
        {
            Debug.Log("Non puoi cominciare il minigioco se sei già impegnato da un'altra parte!");
            return;
        }

        //Imposta il gameState generale in modo che non può iniziare altri minigiochi
        director.setGameState(GameDirector.GameState.ObstacleCourse);

        // -------------------------------------------------------------------- //
        //TIMER
        //Resetta il timer (riparte appena si supera il primo ostacolo)
        timer.gameObject.SetActive(true);
        currentTenths = 0;

        // -------------------------------------------------------------------- //
        //TARGETS
        //Resetta il numero di targets e rende il primo target attivo
        targetNumber = 0;
        Transform[] targets = GameObject.Find("Targets").GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < targets.Length; i++)
        {
            //Riattiva tutti i target (Nota: GameObject.Find() funziona solo con oggetti attivi.
            //Li ho dovuti riattivare tramite GetComponentsInChildren().
            targets[i].gameObject.SetActive(true);
        }
        for(int i = 0;i <= 28; i++)
        {
            string targetName = "Target" + i;
            if(i==0)
                GameObject.Find(targetName).GetComponent<MeshRenderer>().material = activeMaterial;
            else
                GameObject.Find(targetName).GetComponent<MeshRenderer>().material = inactiveMaterial;
        }
    }
    IEnumerator Timer()
    {
        for (currentTenths = 0; currentTenths < 18000; currentTenths++)
        {
            timer.SetText(TimeToString());
            yield return new WaitForSeconds(.1f);
        }
    }
    public void TargetCollision(string targetName) //Metodo chiamato da TurtleController
    {
        Debug.Log(TimeToString());
        //Check del gamestate, per vedere se è iniziato il minigioco (TODO: disabilitato finché non implemento il prompt, altrimenti va in conflitto con l'awake del gamedirector
        if (director.getGameState() != GameDirector.GameState.ObstacleCourse)
            return;

            //Imposta il target successivo come attivo
            if(targetNumber <= 28)
            {
                
                if (targetName != "Target" + targetNumber)
                    return;

                string nextTargetName = "Target" + (targetNumber + 1);
                GameObject.Find(targetName).gameObject.SetActive(false);
                if(targetNumber == 0)
                {
                    StartCoroutine(Timer());
                }
                if(targetNumber == 28)
                {
                    Victory();
                    StopCoroutine(Timer());
                    timer.gameObject.SetActive(false);
                return;
                }
                GameObject.Find(nextTargetName).GetComponent<MeshRenderer>().material = activeMaterial;
                targetNumber++;
                
            }
    }
    public void Victory()
    {
        director.setGameState(GameDirector.GameState.FreeRoaming);

        float earnedPearlsFloat = 10 + 10 * (float)(800/currentTenths); //da più gemme se il percorso è completato entro un minuto e venti
        int earnedPearls = (int)earnedPearlsFloat;
        //current range di perle: 20 max, 13 min
        director.addPearls(earnedPearls);
        director.addOxygenLevel(20);

        canvas.transform.Find("VictoryPanel").gameObject.SetActive(true);
        rewardsText = canvas.transform.Find("VictoryPanel/RewardsPanel/RewardsText").GetComponent<TextMeshProUGUI>();
        rewardsText.SetText("Tempo impiegato: " + TimeToString() + "\n" +
                            "Perle guadagnate: " + earnedPearls + "\n" + 
                            "Livello di ossigeno aumentato del 20%");
    }

    private string TimeToString()
    {
        int minutes = currentTenths / 600;
        int seconds = (currentTenths % 600) / 10;
        int tenths = currentTenths % 10;

        if(seconds<10)
            return minutes + ":0" + seconds + ":" + tenths + "0";
        else
            return minutes + ":" + seconds + ":" + tenths + "0";

    }

}
