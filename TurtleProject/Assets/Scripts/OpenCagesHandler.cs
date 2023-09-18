using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenCagesHandler : MonoBehaviour
{
    [SerializeField] private GameObject container;  //è l'oggetto stesso

    private GameObject canvas;
    private TextMeshProUGUI timer_text, crub_text;
    private Image crub_icon;

    private float timeRemaining;
    private float seconds;

    private bool hasKey;
    private int openCages;
    private int totCages;

    void Awake()
    {
        this.totCages = this.container.GetComponent<SpawnCages>().totalCages;  //prendo il numero di casse
        this.canvas = GameObject.Find("Canvas");
        this.timer_text = canvas.transform.Find("MazeCanvas/TimerText").gameObject.GetComponent<TextMeshProUGUI>();
        this.crub_icon = canvas.transform.Find("MazeCanvas/CrubIcon").gameObject.GetComponent<Image>();
        this.crub_text = canvas.transform.Find("MazeCanvas/FreedCrub").gameObject.GetComponent<TextMeshProUGUI>();
        restartMazeGame();
    }

    //TODO: richiama ogni volta che parte minigame MazeExploring
    public void restartMazeGame()
    {
        this.timeRemaining = 60f;
        this.seconds = Mathf.Round(timeRemaining);

        this.hasKey = false;
        this.openCages = 0;

        timer_text.enabled = true;
        timer_text.SetText(seconds.ToString());

        crub_icon.enabled = true;

        crub_text.enabled = true;
        crub_text.SetText(openCages.ToString() + "/" + totCages.ToString());

        //TODO: fai partire Coroutine(?)
    }

    //TODO: elimina metodo Update, NON è necessario --> sistema codice altrove (Coroutines ogni 0.1f)
    private void Update()
    {
        //if (GameObject.Find("Director").GetComponent<GameDirector>().getGameState() != GameDirector.GameState.MazeExploring)
            //return;

        if (this.timeRemaining > 0)
        {
            this.timeRemaining -= Time.deltaTime;
            this.seconds = Mathf.Round(timeRemaining);
            this.timer_text.SetText(seconds.ToString());
        }

        //TODO: metti dentro Coroutine
        if(IsFinished())   //--> se gioco finito
        {
            //Debug.Log("THE END");
            this.timer_text.enabled = false;
            this.crub_text.enabled = false;
            this.crub_icon.enabled = false;
            this.hasKey = false;

            //faccio scomparire le chiavi
            GameObject[] arr_keys = GameObject.FindGameObjectsWithTag("Chiave");
            for(int i = 0; i<arr_keys.Length; i++)
            {
                Destroy(arr_keys[i]);
            }

            //ANIMAZIONE gabbie che salgono 
            GameObject[] arr_cages = GameObject.FindGameObjectsWithTag("Gabbia");

            if(arr_cages != null)
            {
                for (int i = 0; i < arr_cages.Length; i++)
                {
                    if (arr_cages[i] != null)
                    {
                        arr_cages[i].GetComponent<CageScript>().GoUp();
                        /*if(arr_cages[i].transform.position.y > 50f)
                        {
                            //arr_cages[i].GetComponent<Rigidbody>().isKinematic = true;      //ho già disabilitato la fisica per l'oggetto
                            Destroy(arr_cages[i]);
                        }*/
                    }

                }
            }


        }
    }



    public void OnTriggerStay(Collider other)
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

    //TODO: temporaneo (metodo chiamato da TurtleController
    public void TriggerMethod(Collider other)
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
