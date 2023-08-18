using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabsRandomly : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnAreaWidth = 100f; // Larghezza dell'area in cui verranno spawnati i prefabs
    public float spawnAreaLength = 100f; // Lunghezza dell'area in cui verranno spawnati i prefabs
    public int numerodaspawnare =10;
    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        for (int i = 0; i < numerodaspawnare; i++) // Numero di prefabs da spawnare
        {
            Vector3 randomPosition = new Vector3( Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),Random.Range(80f,100f), Random.Range(-spawnAreaLength / 2, spawnAreaLength / 2 ));

            Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        }
    }
}






