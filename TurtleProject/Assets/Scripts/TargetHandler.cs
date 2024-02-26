using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TargetHandler : MonoBehaviour
{
    private GameObject player;

    //TODO: rimuovi una volta finito con MinigameInterface()
    private GameObject canvas;
    //private GameObject[] targets;
    [SerializeField] private Material activeMaterial, inactiveMaterial;
    private int currentTime;        //Tempo attuale del timer in millisecondi
    
    private int currentTenths;      //Numero di ostacoli superati dal giocatore
    public int targetNumber;        //L'indice di questo ostacolo, per sapere quando deve diventare attivo
    private bool specialTargetActive;

    private Rigidbody rb;

    private Transform obstacleRacePrompt;
    private TextMeshProUGUI NPCName, dialogueText, rewardsText, timer;
    //private Button confirmButton, cancelButton;

    private AudioManager audioManager;

    private void Awake()
    {
        // -------------------------------------------------------------------- //
        //Trova gli oggetti di gioco e di interfaccia all'avvio
        player = GameObject.Find("TurtlePlayer");
        canvas = GameObject.Find("Canvas");

        obstacleRacePrompt = canvas.transform.Find("ObstacleRacePrompt");
        obstacleRacePrompt.gameObject.SetActive(false);
        NPCName = canvas.transform.Find("DialoguePanel/TitlePanel/NPCName").gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = canvas.transform.Find("DialoguePanel/DialogueText").gameObject.GetComponent<TextMeshProUGUI>();
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        //confirmButton = canvas.transform.Find("DialoguePanel/ConfirmRaceButton").gameObject.GetComponent<Button>();
        //cancelButton = canvas.transform.Find("DialoguePanel/CancelRaceButton").gameObject.GetComponent<Button>();
        timer = canvas.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        specialTargetActive = false;
        
    }
    // -------------------------------------------------------------------- //
    //Funzioni chiamate all'interno di TurtleController per gestire il dialogo con l'anguilla
    public void raceStartPrompt()
    {
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
            return;
        if(!canvas.transform.Find("DialoguePanel").gameObject.activeSelf)
            obstacleRacePrompt.gameObject.SetActive(true);
        if(Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("BarsPanel").gameObject.SetActive(false);
            obstacleRacePrompt.gameObject.SetActive(false);
            canvas.transform.Find("DialoguePanel").gameObject.SetActive(true);
            GameDirector.Instance.checkDialoguePanelButtons("ObstacleRace");
            NPCName.SetText("Anguilla");
            dialogueText.SetText("Hey, tu! Sembri una tipa molto in forma. Ti andrebbe di aiutarmi con una faccenda?\n" +
                                 "Le alghe in questo canyon sono in acqua stagnante... Svegliale attraversando tutti gli anelli rocciosi!\n" +
                                 "Sono sicura che il livello di ossigeno aumentera'... E se vai abbastanza veloce, ti daro' anche qualche perla in piu'. Ci stai?");
            /*Aggiunge i listener ai bottoni di dialogo
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmButton_onClick);
            cancelButton.onClick.AddListener(CancelButton_onClick);
            */
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
    // -------------------------------------------------------------------- //
    //Listener dei bottoni di dialogo
    public void ConfirmButton_onClick()
    {
        canvas.transform.Find("DialoguePanel").gameObject.SetActive(false);
        obstacleRacePrompt.gameObject.SetActive(false);
        canvas.transform.Find("BarsPanel").gameObject.SetActive(true);
        raceStart();
        audioManager.PlaySFX(audioManager.selection);
        audioManager.ChangeMusic(audioManager.MatteoGameSountrack,true, 0.12f);
    }
    public void CancelButton_onClick()
    {
        AnguillaTriggerExit();
        obstacleRacePrompt.gameObject.SetActive(true);
        audioManager.PlaySFX(audioManager.selection);
    }
    // -------------------------------------------------------------------- //
    //Funzione di start del minigioco
    public void raceStart()
    {
        // -------------------------------------------------------------------- //
        //GAMESTATE
        //Check per vedere se un altro minigioco � attivo
        //NOTA: ho dovuto mettere questo check in una coroutine perch�, per un mistero quantico, metterlo direttamente in questo metodo
        StartCoroutine(CheckGameState());

        //Imposta il gameState generale in modo che non pu� iniziare altri minigiochi
        GameDirector.Instance.setGameState(GameDirector.GameState.ObstacleCourse);

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
    // -------------------------------------------------------------------- //
    //Coroutine del timer
    IEnumerator Timer()
    {
        Debug.Log("Cominciato Timer ObstacleCourse");
        currentTenths = 0;
        while(currentTenths<=18000)
        {
            GameObject.Find("Canvas/MinigamePanel").GetComponent<MinigameInterface>().setTimerText(currentTenths);
            //timer.SetText(TimeToString());
            currentTenths++;
            if (GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming)
                break;
            yield return new WaitForSeconds(.1f);
        }
    }

    //Coroutine del check del gamestate
    IEnumerator CheckGameState()
    {
        yield return new WaitForFixedUpdate();
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
        {
            Debug.Log(GameDirector.Instance.getGameState());
            
        }
    }
    // -------------------------------------------------------------------- //
    //Metodo chiamato da TurtleController.OnTriggerEnter
    public void TargetCollision(string targetName) 
    {
        //Check del gamestate, per vedere se � iniziato il minigioco
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.ObstacleCourse)
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
                Debug.Log("Attraversato primo anello");
                    StartCoroutine(Timer());
                    audioManager.PlaySFX(audioManager.startRace);

            }
                if(targetNumber >= 28)
                {
                    Victory();
                    StopCoroutine(Timer());
                    //timer.gameObject.SetActive(false);
                    audioManager.PlaySFX(audioManager.endMiniGame);
                    audioManager.ChangeMusic(audioManager.MatteoGameSountrack, false,0.12f);
                    return;
                }
                GameObject.Find(nextTargetName).GetComponent<MeshRenderer>().material = activeMaterial;
                if(targetNumber > 0 && targetNumber < 28)
                {
                    audioManager.PlaySFX(audioManager.crossRing);
                }

            targetNumber++;
                
            }
    }
    // -------------------------------------------------------------------- //
    //Metodo di fine minigioco (TODO: metti in script separato)
    public void Victory()
    {
        GameDirector.Instance.setGameState(GameDirector.GameState.FreeRoaming);

        float earnedPearlsFloat = 10 + 10 * 800f/currentTenths; //da piu' gemme se il percorso e' completato entro un minuto e venti
        int earnedPearls = (int)earnedPearlsFloat;
        //current range di perle: 20 max, 13 min
        GameDirector.Instance.addPearls(earnedPearls);
        GameDirector.Instance.addOxygenLevel(20);

        canvas.transform.Find("VictoryPanel").gameObject.SetActive(true);
        rewardsText = canvas.transform.Find("VictoryPanel/RewardsPanel/RewardsText").GetComponent<TextMeshProUGUI>();
        rewardsText.SetText("Tempo impiegato: " + currentTenths + "\n" +
                            "Perle guadagnate: " + earnedPearls + "\n" + 
                            "Livello di ossigeno aumentato del 20%");
    }

    // -------------------------------------------------------------------- //
    //Shhhh...
    public void summonSpecialTarget()
    {
        canvas.transform.Find("SpecialTargetPrompt").gameObject.SetActive(true);
        if(Input.GetKey(KeyCode.E) && !specialTargetActive)
        {
            specialTargetActive = true;
            canvas.transform.Find("SpecialTargetPrompt").gameObject.SetActive(false);
            audioManager.ChangeMusic(audioManager.Crush_Spawn, true, 0.5f);
            StartCoroutine(summoningRitual());

        }
    }
    public IEnumerator summoningRitual()
    {
        
        GameObject Scorza = GameObject.Find("SpecialTarget");
        Scorza.GetComponentInChildren<MeshRenderer>().enabled = true;
        while (Scorza.transform.position.y>40)
        {
            Scorza.transform.position = Vector3.MoveTowards(Scorza.transform.position, new Vector3(57, 40, 530), Time.deltaTime * 3f);
            yield return new WaitForFixedUpdate();
        }
        audioManager.ChangeMusic(audioManager.Crush_Spawn, false, 0.5f);
    }


}
