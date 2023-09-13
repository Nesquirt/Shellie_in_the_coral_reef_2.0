using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnKey : MonoBehaviour
{
    [SerializeField] private GameObject cage;
    [SerializeField] private GameObject key;
    [SerializeField] private Transform cages;
    [SerializeField] private Transform keys;
    private int numGabbie = 4;

    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponent<Transform>();

        for(int i=0; i<numGabbie; i++)
        {
            Vector3 v = new Vector3( Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1.2f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            var newCage = Instantiate(cage, v, Quaternion.identity);
            newCage.transform.SetParent(cages);

            Vector3 v1 = new Vector3(Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1.2f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            var newKey = Instantiate(key, v1, Quaternion.identity);
            newKey.transform.SetParent(keys);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
