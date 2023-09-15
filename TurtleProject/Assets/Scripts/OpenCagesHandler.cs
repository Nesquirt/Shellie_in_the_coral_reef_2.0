using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class OpenCagesHandler : MonoBehaviour
{
    [SerializeField] private GameObject piano;

    private bool hasKey = false;
    private int openCages = 0;
    private int totCages;

    void Awake()
    {
        this.totCages = this.piano.GetComponent<SpawnCages>().totalCages;
        //this.openCages = 0;
        /*GameObject[] keys = GameObject.FindGameObjectsWithTag("Chiave");
        this.countKey = keys.Length;                   --> mi da 0, poco dopo mi da 4 */

    }

    private void Update()
    {
        if(IsFinished())   //--> se gioco finito
        {
            //ANIMAZIONE gabbie che salgono
        }
    }

    /*
    void FixedUpdate()
    {
        if(IsGameOver())
        {
            //finisce il minigioco
            //tutte le gabbie risalgono
        }

        //quando tartaruga è vicino alla chiave
        if (nearKey == true)
        {
            //Debug.Log("vicino chiave");

            if (Input.GetKeyDown(KeyCode.E))
            {
                 if (hasKey == false)
                {
                    Debug.Log("chiave presa");
                    if(currentKey != null)
                    {
                        Destroy(currentKey.gameObject);
                    }
                    this.hasKey = true;
                    this.countKey--;
                    Debug.Log("CHIAVI: " + countKey);
                    if(countKey == 0)
                    {
                        //togli immagine chiave da UI
                    }

                }
                 else
                {
                    Debug.Log("hai già la chiave");   //solo una chiave alla volta
                }
                
            }
        }

        //quando tartaruga è vicino alla gabbia
        if (nearCage == true)
        {
            if (Input.GetKeyDown(KeyCode.E))   
            {
                if (this.hasKey == true)
                {             
                    Debug.Log("libera granchio");
                    //ANIMAZIONE apertura gabbia
                    //this.openCages++;
                    //Debug.Log("GABBIE: " + openCages);
                    this.hasKey = false;
                }
                else
                    Debug.Log("non hai la chiave");

            }

        }
    } */

    private void CollectKey()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chiave"))
        {
            //other.gameObject.SetActive(false);
            //this.nearKey = true;
            //this.currentKey = other;
            //Debug.Log(currentKey.gameObject);
        }

        if(other.CompareTag("Gabbia"))
        {
            //this.nearCage = true;
            //this.currentCage = other;
            Debug.Log("sono Gabbia");
            //this.currentClosed = currentCage.GetComponent<Cage>().closed;
          


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Chiave"))
        {
            if (Input.GetKeyDown(KeyCode.E) && this.hasKey == false)
            {
                Debug.Log("chiave presa");
                this.hasKey = true;
                Destroy(other.gameObject);
                Debug.Log("ho la chiave");
            }
        }

        if (other.CompareTag("Gabbia"))
        {
            if (Input.GetKeyDown(KeyCode.E) && this.hasKey == true && other.GetComponent<CageScript>().isLocked)
            {
                Debug.Log("apro gabbia");
                other.GetComponent<CageScript>().OpenCage();
                this.hasKey = false;
                this.openCages++;
            }
        }
           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Chiave")
        {
            //this.nearKey = false;
        }

        if (other.gameObject.tag == "Gabbia")
        {
            //this.nearCage = false;
        }
    }

    private bool IsFinished()
    {
        if(this.openCages == this.totCages)
         {
             return true;
         }
        /*else if           //se finisce il timer
           {

           } */
        else
         { 
             return false; 
         }
        


    }

}
