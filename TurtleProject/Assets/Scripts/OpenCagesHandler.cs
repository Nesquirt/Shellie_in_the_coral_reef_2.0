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

    private Collider currentKey;
    private GameObject currentCage;

    private int countKey;
    private int openCages;

    void Awake()
    {
        this.currentKey = null;
        this.currentCage = null;

        //prendo valore variabile tot da script SpawnCages
        this.leggi = piano.GetComponent<SpawnCages>();
        this.countKey = leggi.tot;   
        
        this.openCages = 0;

        Debug.Log("CHIAVI: " + countKey);

        /*GameObject[] keys = GameObject.FindGameObjectsWithTag("Chiave");
        this.countKey = keys.Length;                   --> mi da 0, poco dopo mi da 4 */

    }

    void Update()
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
                    Destroy(currentKey.gameObject);
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
        if(nearCage == true)
        {

            if (Input.GetKeyDown(KeyCode.E))   //AGGIUNGERE controllo: SOLO se la gabbia è ancora chiusa
            {
                if (this.hasKey == true)
                {
                    //Destroy(currentCage);
                    //oppure

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
