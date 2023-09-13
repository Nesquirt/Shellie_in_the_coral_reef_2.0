using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Questo è lo script associato a ogni roccia su cui potrà crescere un corallo
public class CoralHandler : MonoBehaviour
{
    public enum Coral
    {
        PillarCoral,
        FireCoral,
        SoftCoral,
        ElkhornCoral
    }
    public GameObject PillarCoral;
    private Vector3 spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5;
    private int growthCounter;

    [SerializeField] private Canvas canvas;

    private void Start()
    {   
        //Creo i vettori con le posizioni in cui deve instanziare i nuovi coralli relativi alla roccia
        spawnPoint1 = new Vector3(this.transform.position.x + 1.9f, this.transform.position.y + 5.7f, this.transform.position.z + 2.5f);
        spawnPoint2 = new Vector3(this.transform.position.x + 7.6f, this.transform.position.y + 4.3f, this.transform.position.z - 6f);
        spawnPoint3 = new Vector3(this.transform.position.x + 5f, this.transform.position.y + 2.2f, this.transform.position.z -12.3f);
        spawnPoint4 = new Vector3(this.transform.position.x -4.3f, this.transform.position.y + 5.9f, this.transform.position.z + 11.7f);
        spawnPoint5 = new Vector3(this.transform.position.x -7.6f, this.transform.position.y + 4.2f, this.transform.position.z -8);
        growthCounter = 0;
    }

    public void SpawnCorals(Coral coralChoice)
    {
        // -------------------------------------------------------------------- //
        //A seconda del corallo scelto dall'interfaccia, instanzia quattro modelli del corallo scelto negli spawnPoint
        switch (coralChoice)
        {
            case Coral.PillarCoral:
                var newCoral1 = Instantiate(PillarCoral, spawnPoint1, Quaternion.identity);
                newCoral1.transform.parent = this.transform;
                var newCoral2 = Instantiate(PillarCoral, spawnPoint2, Quaternion.identity);
                newCoral2.transform.parent = this.transform;
                var newCoral3 = Instantiate(PillarCoral, spawnPoint3, Quaternion.identity);
                newCoral3.transform.parent = this.transform;
                var newCoral4 = Instantiate(PillarCoral, spawnPoint4, Quaternion.identity);
                newCoral4.transform.parent = this.transform;
                var newCoral5 = Instantiate(PillarCoral, spawnPoint5, Quaternion.identity);
                newCoral5.transform.parent = this.transform;
                break;

        }

        // -------------------------------------------------------------------- //
        //Coroutine per far crescere i coralli nell'arco di un minuto
            StartCoroutine(CoralGrow());

        // -------------------------------------------------------------------- //
    }

    IEnumerator CoralGrow()
    {
        GameObject[] corals = GameObject.FindGameObjectsWithTag("Coral");
        while(growthCounter<=600)
        {
            foreach (GameObject coral in corals)
            {
                Vector3 scaleChange = new Vector3(0.005f, 0.005f, 0.005f);
                coral.transform.localScale += scaleChange;
            }
            growthCounter++;
            yield return new WaitForSeconds(0.1f); 
        }
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //Attiva il prompt "Premi E per piantare un corallo"
            canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(true);

            //TODO: test function temporanea, da eliminare
            //SpawnCorals(Coral.PillarCoral);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(Input.GetKey(KeyCode.E))
        {
            canvas.transform.Find("CoralChoicePanel").gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Uscito dalla zona del CoralSpot, chiude gli elementi dell'UI
        if(canvas.transform.Find("CoralSpotPrompt").gameObject.activeSelf)
        {
            canvas.transform.Find("CoralSpotPrompt").gameObject.SetActive(false);
        }
        if(canvas.transform.Find("CoralChoicePanel").gameObject.activeSelf)
        {
            canvas.transform.Find("CoralChoicePanel").gameObject.SetActive(false);
        }
    }

}
