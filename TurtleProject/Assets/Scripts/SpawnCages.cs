using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnCages : MonoBehaviour
{
    [SerializeField] private GameObject cagePrefab;
    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private Transform cagesParent;
    [SerializeField] private Transform keysParent;

    [SerializeField] public int totalCages = 4;
    //private int timer = 180f;

    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponent<Transform>();

        for(int i=0; i<totalCages; i++)
        {
            Vector3 v = new Vector3( Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1.2f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            GameObject newCage = Instantiate(cagePrefab, v, Quaternion.identity);
            newCage.transform.SetParent(cagesParent);

            Vector3 v1 = new Vector3(Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 0.5f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            GameObject newKey = Instantiate(keyPrefab, v1, Quaternion.identity);
            newKey.transform.SetParent(keysParent);

        }

    }

}
