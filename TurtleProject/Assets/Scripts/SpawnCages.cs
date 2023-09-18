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

    //private Vector3[] mazeSpawnPoints;
    private List<Vector3> mazeSpawnPoints;
    private float posx;
    private float posy;
    private float posz;

    private int min;
    private int max;
    private int num;

    private void Awake()
    {

        //this.mazeSpawnPoints = new Vector3[15];
        this.mazeSpawnPoints = new List<Vector3>();
        this.posx = this.transform.position.x;
        this.posy = this.transform.position.y;
        this.posz = this.transform.position.z;

        mazeSpawnPoints.Add(new Vector3(posx - 54.8f, posy + 10f, posz - 302.6f));
        mazeSpawnPoints.Add(new Vector3(posx + 3.6f, posy + 10f, posz - 298.6f));
        mazeSpawnPoints.Add(new Vector3(posx - 116.8f, posy + 10f, posz - 213.5f));
        mazeSpawnPoints.Add(new Vector3(posx - 58.2f, posy + 10f, posz - 214.4f));
        mazeSpawnPoints.Add(new Vector3(posx - 115.1f, posy + 10f, posz - 86.5f));
        mazeSpawnPoints.Add(new Vector3(posx + 2.8f, posy + 10f, posz - 111.6f));
        mazeSpawnPoints.Add(new Vector3(posx - 52.9f, posy + 10f, posz - 36.9f));
        mazeSpawnPoints.Add(new Vector3(posx - 115.7f, posy + 10f, posz + 1f));
        mazeSpawnPoints.Add(new Vector3(posx - 178.2f, posy + 10f, posz - 71.2f));
        mazeSpawnPoints.Add(new Vector3(posx - 292.6f, posy + 10f, posz - 77.8f));
        mazeSpawnPoints.Add(new Vector3(posx - 371.7f, posy + 10f, posz - 31.4f));
        mazeSpawnPoints.Add(new Vector3(posx - 54.8f, posy + 10f, posz + 7.1f));
        mazeSpawnPoints.Add(new Vector3(posx - 109.9f, posy + 10f, posz - 295.9f));
        mazeSpawnPoints.Add(new Vector3(posx - 4.8f, posy + 10f, posz - 158.6f));
        mazeSpawnPoints.Add(new Vector3(posx, posy + 10f, posz));

        Spawn();
    }

    private void Spawn()
    {
        this.min = 0;
        this.max = mazeSpawnPoints.Count;
        Debug.Log(max);

        for (int i = 0; i < totalCages * 2 - 1; i++)
        {
            this.num = Random.Range(0, mazeSpawnPoints.Count);



        }

    }
}

    /*
    private void SpawnARRAY()
    {
        this.min = 0;
        this.max = mazeSpawnPoints.Length;

        for (int i = 0; i < totalCages * 2; i++)
        {
            this.num = Random.Range(min, max);       //numero random tra 0 e 14
            Debug.Log(num);
            Debug.Log("min: " + min + "; MAX: " + max);
            Instantiate(cagePrefab, mazeSpawnPoints[num], Quaternion.identity, cagesParent);
            for(int j=num; j<mazeSpawnPoints.Length - 1; j++)
            {
                mazeSpawnPoints[num] = mazeSpawnPoints[num + 1];
            }
            mazeSpawnPoints.RemoveAt(mazeSpawnPoints.Length); 
            max--;
        }

    }
    */

    /*
    private void SpawnPROVA()
    {
        for(int i=0; i<mazeSpawnPoints.Length; i++)
        {
            Instantiate(cagePrefab, mazeSpawnPoints[i], Quaternion.identity, cagesParent);
        }
    }


    }
    */


/*
* void Start()
{

 Transform transform = GetComponent<Transform>();

for(int i=0; i<totalCages; i++)
{
    Vector3 v = new Vector3( Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 3f, Random.Range(transform.position.z - 50, transform.position.z + 50));
    GameObject newCage = Instantiate(cagePrefab, v, Quaternion.identity);
    newCage.transform.SetParent(cagesParent);

    Vector3 v1 = new Vector3(Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1f, Random.Range(transform.position.z - 50, transform.position.z + 50));
    GameObject newKey = Instantiate(keyPrefab, v1, Quaternion.identity);
    newKey.transform.SetParent(keysParent);

}

}
*/