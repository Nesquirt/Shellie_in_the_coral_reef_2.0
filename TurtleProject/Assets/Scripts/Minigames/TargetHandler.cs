using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] private Material activeMaterial, inactiveMaterial;
    private int currentTime;        //Tempo attuale del timer in millisecondi
    
    private int currentTenths;      //Numero di ostacoli superati dal giocatore
    public int targetNumber;        //L'indice di questo ostacolo, per sapere quando deve diventare attivo
    private bool specialTargetActive;

    private Rigidbody rb;
    [SerializeField] private UIManager UImanager;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        specialTargetActive = false;   
    }
    // -------------------------------------------------------------------- //
    //Funzione di start del minigioco
    public void raceStart()
    {
        
        //Check per vedere se un altro minigioco � attivo
        if (UImanager.dialogueInterface.getCurrentNPC() == "Anguilla")
        {
            // -------------------------------------------------------------------- //
            //GAMESTATE
            GameDirector.Instance.setGameState(GameDirector.GameState.ObstacleCourse);
            // -------------------------------------------------------------------- //
            //TIMER
            //Resetta il timer (riparte appena si supera il primo ostacolo)
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

            for (int i = 0; i <= 28; i++)
            {
                string targetName = "Target" + i;
                if (i == 0)
                    GameObject.Find(targetName).GetComponent<MeshRenderer>().material = activeMaterial;
                else
                    GameObject.Find(targetName).GetComponent<MeshRenderer>().material = inactiveMaterial;
            }

            //AUDIO
            audioManager.MiniGame();
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
            currentTenths++;
            UImanager.minigameInterface.setTimerText(currentTenths);
            if (GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming)
                break;
            yield return new WaitForSeconds(.1f);
        }
    }

    /*
    //Coroutine del check del gamestate
    IEnumerator CheckGameState()
    {
        yield return new WaitForFixedUpdate();
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.FreeRoaming)
        {
            Debug.Log(GameDirector.Instance.getGameState());
            
        }
    }
    */
    // -------------------------------------------------------------------- //
    //Metodo chiamato da TurtleController.OnTriggerEnter
    public void TargetCollision(string targetName) 
    {
        //Check del gamestate, per vedere se � iniziato il minigioco
        if (GameDirector.Instance.getGameState() != GameDirector.GameState.ObstacleCourse)
            return;

        //Imposta il target successivo come attivo
        UImanager.minigameInterface.setScoreText(targetNumber + 1, 29);
        if (targetNumber <= 28)
            {
                if (targetName != "Target" + targetNumber)
                    return;

                string nextTargetName = "Target" + (targetNumber + 1);
                GameObject.Find(targetName).gameObject.SetActive(false);
                

                if (targetNumber == 0)
                {
                    Debug.Log("Attraversato primo anello");
                    StartCoroutine(Timer());
                }
                if(targetNumber >= 28)
                {
                    Victory();
                    StopCoroutine(Timer());
                    audioManager.MiniGame();
                    return;
                }
                GameObject.Find(nextTargetName).GetComponent<MeshRenderer>().material = activeMaterial;

                audioManager.CrossRing(targetNumber);
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
        GameDirector.Instance.addParameters(0, 25, 0);
        UImanager.victoryInterface.setRewardsText("Tempo impiegato: " + currentTenths + "\n" +
                            "Perle guadagnate: " + earnedPearls + "\n" +
                            "Livello di ossigeno aumentato del 20%");
        Debug.Log("TestVittoria");
        UImanager.victoryInterface.toggleVictoryPanel(true);
    }

    // -------------------------------------------------------------------- //
    //Shhhh...
    /*
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
    */

}
