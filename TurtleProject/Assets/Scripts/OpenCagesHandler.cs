using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class OpenCagesHandler : MonoBehaviour
{
    [SerializeField] private GameObject piano;

    private SpawnCages leggi;

    private bool hasKey = false;
    private bool nearKey = false;
    private bool nearCage = false;

    private bool gameOver = false;
    private bool currentClosed;

    private Collider currentKey;
    public Collider currentCage;

    private int countKey;
    private int openCages;

    void Awake()
    {
        this.currentKey = null;
        this.currentCage = null;
        this.currentClosed = true;

        //prendo valore variabile tot da script SpawnCages
        this.leggi = piano.GetComponent<SpawnCages>();
        this.countKey = leggi.tot;   
        
        this.openCages = 0;

        Debug.Log("CHIAVI: " + countKey);

        /*GameObject[] keys = GameObject.FindGameObjectsWithTag("Chiave");
        this.countKey = keys.Length;                   --> mi da 0, poco dopo mi da 4 */

    }

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
                //Debug.Log(currentClosed);
                //this.currentClosed = this.currentCage.GetComponent<Cage>().closed;

                if (this.hasKey == true)
                {             
                    //this.currentClosed = false;
                    //this.currentCage.GetComponent<Cage>().closed = false;
                    Debug.Log("libera granchio");
                    //ANIMAZIONE apertura gabbia
                    this.openCages++;
                    Debug.Log("GABBIE: " + openCages);
                    this.hasKey = false;
                }
                else
                    Debug.Log("non hai la chiave");

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Chiave"))
        {
            this.nearKey = true;
            this.currentKey = other;
            //Debug.Log(currentKey.gameObject);
        }

        if(other.gameObject.tag == "Gabbia")
        {
            if (currentCage != other)
            {
                this.nearCage = true;
                this.currentCage = other;
                Debug.Log("ciao");
                //this.currentClosed = currentCage.GetComponent<Cage>().closed;
                
            }


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

    private bool IsGameOver()
    {
       if(this.openCages == this.countKey)
        {
            return true;
        }
      /* else if           //se finisce il timer
          {

          } */
       else
        { 
            return false; 
        }
       

    }

}
