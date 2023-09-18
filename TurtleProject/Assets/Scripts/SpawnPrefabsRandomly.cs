using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SpawnPrefabsRandomly : MonoBehaviour
{ 
    
    [SerializeField] private GameObject prefabToSpawn1;
    [SerializeField] private GameObject prefabToSpawn2;
    [SerializeField] private GameObject prefabToSpawn3;
    [SerializeField] private GameObject sostegno;
    [SerializeField] private float spawnAreaWidth = 100f; // Larghezza dell'area in cui verranno spawnati i prefabs
    [SerializeField] private float spawnAreaLength = 100f; // Lunghezza dell'area in cui verranno spawnati i prefabs
    [SerializeField] private int numerodaspawnare =10;
    private float originepianox;
    private  float originepianoz;
    public float lunghezzapiano;
    public float larghezzapiano;
    private static int oggettiNelPiano ;
    public List<GameObject> spazzature = new List<GameObject>();
    public Rigidbody rb ;
    public float upwardForce;
    public int a ,b;
    public TextMeshProUGUI testo;
    public TextMeshProUGUI testo1;
    public TextMeshProUGUI testo2;
     public Image img;
    public float totalTime = 60f;
    public float totalTime2;// Il tempo totale del timer in secondi.
    private float currentTime;
    private float currentTime2;
    public int rifiutiraccolti;
   
    void Start1()
    { 
            a=0;
            b=0;
         
           // totalTime=60f;
           // rifiutiraccolti=0;
            //numerodaspawnare=5;
            sostegno.gameObject.SetActive(false);
            testo.gameObject.SetActive(true);
            testo1.gameObject.SetActive(true);
            testo2.gameObject.SetActive(true);
            img.gameObject.SetActive(true);
            totalTime2=totalTime+10f;
           oggettiNelPiano = 0;
           currentTime = totalTime;
           currentTime2=totalTime2;
          Transform tp = GetComponent<Transform>();
          originepianox=tp.position.x;
          originepianoz=tp.position.z;
         
          SpawnPrefabs();
    }
    void Update(){

        
         Debug.Log("oggetti nel paino"+oggettiNelPiano);
         Debug.Log("oggetti raccolti"+rifiutiraccolti);
        if(currentTime>0){
            testo.text = Mathf.Round(currentTime).ToString();
            testo1.text = oggettiNelPiano.ToString();
             
            currentTime -= Time.deltaTime;
        }
        if (currentTime2>0){
             testo1.text = oggettiNelPiano.ToString();
             currentTime2 -=Time.deltaTime;
        }
         //currentTime2 -=Time.deltaTime;
        
         /*if (oggettiNelPiano==numerodaspawnare||a==1){
             a=1;
            rb.AddForce(Vector3.up * upwardForce);
         }*/
         if (currentTime <= 0 && a==0)
        {    
            if (b==0){
               rifiutiraccolti=oggettiNelPiano; 
               b=1;
            }
            rb.AddForce(Vector3.up * upwardForce);
            testo.gameObject.SetActive(false);
            testo1.gameObject.SetActive(false);
            testo2.gameObject.SetActive(false);
            img.gameObject.SetActive(false);
            currentTime = 0;
             //rb.AddForce(Vector3.up * upwardForce);
            // Esegui azioni quando il timer scade.
            // Ad esempio, puoi terminare il gioco o attivare un'altra logica.
        }
        if (currentTime2<=0){

          foreach (GameObject spazzatura in spazzature)
            {
                Destroy(spazzatura);
            }
            spazzature.Clear();
            //testo.text = " ";
            //testo1.text = " ";
           a=1;
           currentTime2=0;
           sostegno.gameObject.SetActive(true);
           this.gameObject.SetActive(false);

           
          
        }

          /*  if (oggettiNelPiano==numerodaspawnare)
            {
                Debug.Log("spazzatura raccolta");
             foreach (GameObject spazzatura in spazzature)
            {
                Destroy(spazzatura);
            }

            
            spazzature.Clear();
            }*/


        

    }
  

    void SpawnPrefabs()
    {
       
        for (int i = 0; i < numerodaspawnare; i++) // Numero di prefabs da spawnare
        {
            Vector3 randomPosition = new Vector3( originepianox+lunghezzapiano,Random.Range(80f,100f), Random.Range(originepianoz-spawnAreaLength/2,originepianoz+spawnAreaLength / 2 ));
            Vector3 randomPosition1 = new Vector3( Random.Range(originepianox-spawnAreaLength/2,originepianox+spawnAreaLength / 2 ),Random.Range(80f,100f), originepianoz+lunghezzapiano);
            Vector3 randomPosition2 = new Vector3( originepianox-lunghezzapiano,Random.Range(80f,100f), Random.Range(originepianoz-spawnAreaLength/2,originepianoz+spawnAreaLength / 2 ));
            Vector3 randomPosition3 = new Vector3( Random.Range(originepianox-spawnAreaLength/2,originepianox+spawnAreaLength / 2 ),Random.Range(80f,100f), originepianoz-lunghezzapiano);
            
            if (i<2){
                if(i%2==0){
                    spazzature.Add(Instantiate(prefabToSpawn1, randomPosition, Quaternion.identity));
                }else{
              spazzature.Add(Instantiate(prefabToSpawn1, randomPosition1, Quaternion.identity));  
                }
            }
            else if(i>=2&&i<4){
                if(i%2==0){
                    spazzature.Add(Instantiate(prefabToSpawn2, randomPosition2, Quaternion.identity));

                }else{
                        spazzature.Add(Instantiate(prefabToSpawn2, randomPosition3, Quaternion.identity));
                }
                  
            }
            else{
                spazzature.Add(Instantiate(prefabToSpawn3, randomPosition, Quaternion.identity));
            }
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("spazzatura"))
        {
            oggettiNelPiano++;
            //Debug.Log("Oggetto entrato nel piano. Oggetti totali nel piano: " + oggettiNelPiano);
        }
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("spazzatura"))
        {
            oggettiNelPiano--;
           // Debug.Log("Oggetto uscito dal piano. Oggetti totali nel piano: " + oggettiNelPiano);
        }
    }
      
    private void OnEnable()
    {
       Start1();
    }

     
}






