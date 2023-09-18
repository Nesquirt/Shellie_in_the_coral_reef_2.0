using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject directorObject;
    private GameDirector director;
    //private GameObject[] targets;
    [SerializeField] private Material activeMaterial, inactiveMaterial;
    private int currentTime;        //Tempo attuale del timer in millisecondi

    private int currentTenths;      //Numero di ostacoli superati dal giocatore
    public int targetNumber;        //L'indice di questo ostacolo, per sapere quando deve diventare attivo

    private Rigidbody rb;

    private void Awake()
    {
        director = directorObject.GetComponent<GameDirector>();
        //targets = GameObject.FindGameObjectsWithTag("Target");

        raceStart(); //TODO: rimuovi e mettilo come prompt
    }

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
        //Resetta il timer e lo fa ripartire
        currentTenths = 0;
        StartCoroutine(Timer());

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
        for (currentTenths = 0; currentTenths < 1800; currentTenths++) //600 = 1 minuto
        {
            if (currentTenths % 10 == 0)
            {

            }
            yield return new WaitForSeconds(.1f);
        }
    }

    public void TargetCollision(string targetName) //Metodo chiamato da TurtleController
    {
        //Check del gamestate, per vedere se è iniziato il minigioco (TODO: disabilitato finché non implemento il prompt, altrimenti va in conflitto con l'awake del gamedirector
        //if (director.getGameState() != GameDirector.GameState.ObstacleCourse)
            //return;

            //Imposta il target successivo come attivo
            if(targetNumber <= 28)
            {
                
                if (targetName != "Target" + targetNumber)
                    return;

                string nextTargetName = "Target" + (targetNumber + 1);
                GameObject.Find(targetName).gameObject.SetActive(false);
                if(targetNumber == 28)
                {
                    Victory();
                    return;
                }
                GameObject.Find(nextTargetName).GetComponent<MeshRenderer>().material = activeMaterial;
                targetNumber++;
                
            }
    }

    public void Victory()
    {
        director.setGameState(GameDirector.GameState.FreeRoaming);
        director.addPearls(15);
        director.addOxygenLevel(20);
    }

}
