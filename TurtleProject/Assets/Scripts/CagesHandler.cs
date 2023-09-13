using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CagesHandler : MonoBehaviour
{
    private bool hasKey = false;
    private bool nearKey = false;
    private bool nearCage = false;

    [SerializeField] private Collider currentKey = null;
    [SerializeField] private GameObject currentCage = null;

    private int countKey;


    void Awake()
    {
        this.countKey = SpawnCages.totCages;
        Debug.Log("CHIAVI: " + countKey);

        /*GameObject[] keys = GameObject.FindGameObjectsWithTag("Chiave");
        this.countKey = keys.Length;           --> mi da 0, poco dopo mi da 4 */

    }

    void Update()
    {

        //quando tartaruga è vicino alla chiave
        if (nearKey == true)
        {
            //Debug.Log("vicino chiave");
            if (Input.GetKeyDown(KeyCode.E))
            {
                 if (hasKey == false)
                {
                    Debug.Log("chiave presa");
                    Destroy(currentKey.gameObject);
                    this.hasKey = true;
                    this.countKey--;
                    Debug.Log("CHIAVI: " + countKey);
                }
                 else
                {
                    Debug.Log("hai già la chiave");   //solo una chiave alla volta
                }
                
            }

        }

        //quando tartaruga è vicino alla gabbia
        if(nearCage == true)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (this.hasKey == true)
                {
                    Debug.Log("libera pesce");
                    this.hasKey = false;
                }
                else
                    Debug.Log("non hai la chiave");

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chiave")
        {
            this.nearKey = true;
            this.currentKey = other;
            //Debug.Log(currentKey.gameObject);
        }

        if(other.gameObject.tag == "Gabbia")
        {
            this.nearCage = true;
            this.currentCage = other.GetComponent<GameObject>();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Chiave")
        {
            this.nearKey = false;
        }

        if (other.gameObject.tag == "Gabbia")
        {
            this.nearCage = false;
        }
    }

}
