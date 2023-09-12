using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnKey : MonoBehaviour
{
    [SerializeField] private GameObject gabbia;
    [SerializeField] private GameObject chiave;
    //[SerializeField] private GameObject turtle;
    public int numGabbie;
    //public int 

    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponent<Transform>();
        //float size = transform.position.x - transform.localScale.x/2;
        //Debug.Log("x: " + transform.localPosition.x );
        //Debug.Log("y: " + transform.localScale.y);

        for(int i=0; i<numGabbie; i++)
        {
            Vector3 v = new Vector3( Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1.2f, Random.Range(transform.position.z - 50, transform.position.z + 50));
            //Debug.Log(v);
            Instantiate(gabbia, v, Quaternion.identity);
        }

        //Vector3 v = new Vector3(Random.Range(transform.position.x - 50, transform.position.x + 50), transform.position.y + 1.2f, Random.Range(transform.position.z - 50, transform.position.z + 50));
        //Instantiate(gabbia, v, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
