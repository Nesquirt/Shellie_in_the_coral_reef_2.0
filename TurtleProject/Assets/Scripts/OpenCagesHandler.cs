using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCagesHandler : MonoBehaviour
{
    [SerializeField] private GameObject piano;

    [SerializeField] private TextMeshProUGUI timer_text;
    [SerializeField] private TextMeshProUGUI crub_text;
    [SerializeField] private Image crub_icon; 

    private float timeRemaining;
    private float seconds;

    private bool hasKey;
    private int openCages;
    private int totCages;

    void Awake()
    {
        this.totCages = this.piano.GetComponent<SpawnCages>().totalCages;
        this.timeRemaining = 6f;
        this.seconds = Mathf.Round(timeRemaining);

        this.hasKey = false;
        this.openCages = 0;

        this.timer_text.enabled = true;
        this.timer_text.SetText(seconds.ToString());

        this.crub_text.enabled = true;
        this.crub_icon.enabled = true;
        this.crub_text.SetText(openCages.ToString() + "/" + totCages.ToString());

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

        //DISATTIVARE COLLIDER gabbie
        if(IsFinished())   //--> se gioco finito
        {
            Debug.Log("THE END");
            this.timer_text.enabled = false;
            this.crub_text.enabled = false;
            this.crub_icon.enabled = false;

            //faccio scomparire le chiavi
            GameObject[] arr_keys = GameObject.FindGameObjectsWithTag("Chiave");
            for(int i = 0; i<arr_keys.Length; i++)
            {
                Destroy(arr_keys[i]);
            }

            //ANIMAZIONE gabbie che salgono 
            GameObject[] arr_cages = GameObject.FindGameObjectsWithTag("Gabbia");
            for(int i = 0; i<arr_cages.Length; i++)
            {
                arr_cages[i].GetComponent<CageScript>().GoUp();
            }

        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Chiave"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!hasKey)
                {
                    Destroy(other.gameObject);
                    this.hasKey = true;
                    Debug.Log("ho la chiave");
                }
                else
                    return;
            }
        }

        if (other.CompareTag("Gabbia"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (hasKey && other.GetComponent<CageScript>().isLocked)
                {
                    Debug.Log("apro gabbia");
                    other.GetComponent<CageScript>().OpenCage();
                    this.hasKey = false;
                    this.openCages++;
                    Debug.Log("GABBIE APERTE: " + this.openCages);
                    this.crub_text.SetText(openCages.ToString() + "/" + totCages.ToString());
                }
                else
                    return;
                
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
