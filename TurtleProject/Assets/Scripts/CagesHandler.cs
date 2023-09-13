using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagesHandler : MonoBehaviour
{
    private bool hasKey = false;
    private bool nearKey = false;
    private bool nearCage = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //quando tartaruga è vicino alla chiave
        if(nearKey == true)
        {
            //Debug.Log("vicino chiave");
            if (Input.GetKeyDown(KeyCode.E) && hasKey == false)
            {
                Debug.Log("chiave presa");

                this.hasKey = true;
            }

        }

        //se tartaruga è vicino alla gabbia
        if(nearCage == true)
        {
            if (this.hasKey == true)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("libera pesce");
                    this.hasKey = false;
                }
            }
            else
            {
                Debug.Log("non hai la chiave");
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Chiave")
        {
            this.nearKey = true;
        }

        if(other.gameObject.tag == "Gabbia")
        {
            this.nearCage = true;
            
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
