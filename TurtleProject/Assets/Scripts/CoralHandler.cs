using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Questo è lo script associato a ogni roccia su cui potrà crescere un corallo
public class CoralHandler : MonoBehaviour
{
    [SerializeField] private GameObject gameDirector;
    [SerializeField] private GameObject PillarCoral, FireCoral, SoftCoral, ElkhornCoral;
    private Vector3 spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5;
    private Vector3[] spawnPoints;
    private int growthCounter;
    private bool isGrowing;

    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        isGrowing = false;
        spawnPoints = new Vector3[5];
        //Creo i vettori con le posizioni in cui deve instanziare i nuovi coralli relativi alla roccia
        spawnPoints[0] = new Vector3(this.transform.position.x + 1.9f, this.transform.position.y + 5.7f, this.transform.position.z + 2.5f);
        spawnPoints[1] = new Vector3(this.transform.position.x + 7.6f, this.transform.position.y + 4.3f, this.transform.position.z - 6f);
        spawnPoints[2] = new Vector3(this.transform.position.x + 5f, this.transform.position.y + 2.2f, this.transform.position.z -12.3f);
        spawnPoints[3] = new Vector3(this.transform.position.x -4.3f, this.transform.position.y + 5.9f, this.transform.position.z + 11.7f);
        spawnPoints[4] = new Vector3(this.transform.position.x -7.6f, this.transform.position.y + 4.2f, this.transform.position.z -8);

        growthCounter = 0;
    }

    public void SpawnCorals(int coralChoice)
    {
        isGrowing = true;
        // -------------------------------------------------------------------- //
        //A seconda del corallo scelto dall'interfaccia, instanzia cinque modelli del corallo scelto negli spawnPoint
        switch (coralChoice)
        {
            case 0: //PillarCoral
                for(int i = 0; i<spawnPoints.Length; i++)
                {
                    var newCoral = Instantiate(PillarCoral, spawnPoints[i], Quaternion.identity);
                    newCoral.transform.parent = this.transform;
                }
                //TODO: vedi se è possibile implementare i seguenti valori tramite gli ScriptableObjects, invece che a mano
                gameDirector.GetComponent<GameDirector>().modifyBiodiversityChange(3);
                gameDirector.GetComponent<GameDirector>().modifyPollutionChange(1);
                gameDirector.GetComponent<GameDirector>().modifyOxygenLevelChange(3);
                break;
            case 1: //FireCoral
                  
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    var newCoral = Instantiate(FireCoral, spawnPoints[i], Quaternion.identity);
                    newCoral.transform.parent = this.transform;
                }
                //TODO: vedi se è possibile implementare i seguenti valori tramite gli ScriptableObjects, invece che a mano
                gameDirector.GetComponent<GameDirector>().modifyBiodiversityChange(2);
                gameDirector.GetComponent<GameDirector>().modifyPollutionChange(0);
                gameDirector.GetComponent<GameDirector>().modifyOxygenLevelChange(3);
                break;
            case 2: //SoftCoral
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    var newCoral = Instantiate(SoftCoral, spawnPoints[i], Quaternion.identity);
                    newCoral.transform.parent = this.transform;
                }
                //TODO: vedi se è possibile implementare i seguenti valori tramite gli ScriptableObjects, invece che a mano
                gameDirector.GetComponent<GameDirector>().modifyBiodiversityChange(2);
                gameDirector.GetComponent<GameDirector>().modifyPollutionChange(0);
                gameDirector.GetComponent<GameDirector>().modifyOxygenLevelChange(3);
                break;
            case 3: //ElkhornCoral
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    var newCoral = Instantiate(ElkhornCoral, spawnPoints[i], Quaternion.identity);
                    newCoral.transform.parent = this.transform;
                }
                //TODO: vedi se è possibile implementare i seguenti valori tramite gli ScriptableObjects, invece che a mano
                gameDirector.GetComponent<GameDirector>().modifyBiodiversityChange(2);
                gameDirector.GetComponent<GameDirector>().modifyPollutionChange(0);
                gameDirector.GetComponent<GameDirector>().modifyOxygenLevelChange(3);
                break;


        }

        // -------------------------------------------------------------------- //
        //Coroutine per far crescere i coralli nell'arco di un minuto
            StartCoroutine(CoralGrow());

        // -------------------------------------------------------------------- //
    }

    IEnumerator CoralGrow()
    {
        //GameObject[] corals = GetComponentsInChildren<GameObject>();
        while(growthCounter<=600)
        {
            foreach (Transform coral in transform)
            {
                if(coral.tag == "Coral")
                {
                    Vector3 scaleChange = new Vector3(0.005f, 0.005f, 0.005f);
                    coral.transform.localScale += scaleChange;
                }
                
            }
            growthCounter++;
            yield return new WaitForSeconds(0.1f); 
        }
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player" && 
            gameDirector.GetComponent<GameDirector>().getGameState() == GameDirector.GameState.FreeRoaming)
        {
            //Attiva il prompt "Premi E per piantare un corallo"
            canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(true);

            //TODO: test function temporanea, da eliminare
            //SpawnCorals(Coral.PillarCoral);
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player" &&
            gameDirector.GetComponent<GameDirector>().getGameState() == GameDirector.GameState.FreeRoaming)
        {
            if (isGrowing)
            {
                canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(false);
                canvas.transform.Find("CoralChoicePanel").gameObject.SetActive(false);
                return;
            }
            if (Input.GetKey(KeyCode.E))
            {
                gameDirector.GetComponent<GameDirector>().currentCoralSpot = this.gameObject;
                canvas.transform.Find("CoralChoicePanel").gameObject.SetActive(true);
                canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(false);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                canvas.transform.Find("CoralChoicePanel").gameObject.SetActive(false);
            }
            if (!canvas.transform.Find("CoralChoicePanel").gameObject.activeSelf)
            {
                canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player" &&
            gameDirector.GetComponent<GameDirector>().getGameState() == GameDirector.GameState.FreeRoaming)
        {

            //Uscito dalla zona del CoralSpot, chiude gli elementi dell'UI
            if (canvas.transform.Find("CoralSpotPrompt").gameObject.activeSelf)
            {
                canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(false);
            }
            if (canvas.transform.Find("CoralChoicePanel").gameObject.activeSelf)
            {
                canvas.transform.Find("CoralChoicePanel").gameObject.SetActive(false);
            }
        }
    }

}
