using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class avvioTrashColleting : MonoBehaviour
{


    [SerializeField] private GameObject scriptObject ;
    public TextMeshProUGUI testo;
    public TextMeshProUGUI testo1;
    public TextMeshProUGUI testo2;
     public Image img;
    // Start is called before the first frame update
    void Awake(){
    scriptObject.SetActive(false);
    testo.gameObject.SetActive(false);
    testo1.gameObject.SetActive(false);
    testo2.gameObject.SetActive(false);
    img.gameObject.SetActive(false);
     }


private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scriptObject.SetActive(true);
        }
      
    }
    // Update is called once per frame
    
}
