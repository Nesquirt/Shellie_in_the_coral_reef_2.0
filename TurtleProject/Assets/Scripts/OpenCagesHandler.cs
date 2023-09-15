using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCagesHandler : MonoBehaviour
{
    [SerializeField] private GameObject piano;

    [SerializeField] public TextMeshProUGUI timer_text;

    private float timeRemaining = 60f;
    private float seconds;

    private bool hasKey = false;
    private int openCages = 0;
    private int totCages;

    void Awake()
    {
        this.totCages = this.piano.GetComponent<SpawnCages>().totalCages;
        this.seconds = Mathf.Round(timeRemaining);
        //this.openCages = 0;

        /*GameObject[] keys = GameObject.FindGameObjectsWithTag("Chiave");
        this.countKey = keys.Length;                   --> mi da 0, poco dopo mi da 4 */

    }

    private void Update()
    {
        if (this.timeRemaining > 0)
        {
            this.timeRemaining -= Time.deltaTime;
            this.seconds = Mathf.Round(timeRemaining);
            this.timer_text.SetText(seconds.ToString());
        }

        if(IsFinished())   //--> se gioco finito
        {
            Debug.Log("THE END");
            //ANIMAZIONE gabbie che salgono
        }
    }

    

    /*private void CollectKey()
    {

    }*/

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Chiave"))
        {
            if (Input.GetKeyDown(KeyCode.E) && this.hasKey == false)
            {
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
                Debug.Log("GABBIE APERTE: " + this.openCages);
            }
        }
           
    }

    private bool IsFinished()
    {
        if(this.openCages == this.totCages || this.seconds == 0)
         {
             return true;
         }
        else
         { 
             return false; 
         }
    }

}
