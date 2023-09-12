using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4;
    private int growthCounter;

    private void Start()
    {   
        //Creo i vettori con le posizioni in cui deve instanziare i nuovi coralli relativi alla roccia
        spawnPoint1 = new Vector3(this.transform.position.x + 1.9f, this.transform.position.y + 5.7f, this.transform.position.z + 2.5f);
        spawnPoint2 = new Vector3(this.transform.position.x + 7.6f, this.transform.position.y + 4.3f, this.transform.position.z - 6f);
        spawnPoint3 = new Vector3(this.transform.position.x + 5f, this.transform.position.y + 2.2f, this.transform.position.z -12.3f);
        spawnPoint4 = new Vector3(this.transform.position.x -4.3f, this.transform.position.y + 5.9f, this.transform.position.z + 11.7f);

        growthCounter = 0;
    }

    public void SpawnCorals(Coral coralChoice)
    {
        // -------------------------------------------------------------------- //
        //A seconda del corallo scelto dall'interfaccia, instanzia quattro modelli del corallo scelto negli spawnPoint
        switch (coralChoice)
        {
            case Coral.PillarCoral:
                Instantiate(PillarCoral, spawnPoint1, Quaternion.identity);
                Instantiate(PillarCoral, spawnPoint2, Quaternion.identity);
                Instantiate(PillarCoral, spawnPoint3, Quaternion.identity);
                Instantiate(PillarCoral, spawnPoint4, Quaternion.identity);
                break;

        }

        // -------------------------------------------------------------------- //
        //Coroutine per far crescere i coralli nell'arco di un minuto
        StartCoroutine(CoralGrow());

        if (growthCounter >= 60)
            StopCoroutine(CoralGrow());
        // -------------------------------------------------------------------- //
    }

    IEnumerator CoralGrow()
    {
        GameObject[] corals = GameObject.FindGameObjectsWithTag("Coral");
        foreach(GameObject coral in corals)
        {
            Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
            coral.transform.localScale += scaleChange;

            growthCounter++;
        }
        yield return new WaitForSeconds(1); //aspetta 5 secondi prima di ripartire;
    }



}
