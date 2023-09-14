using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnPrefabsRandomly : MonoBehaviour
{ 
    
    public GameObject prefabToSpawn1;
    public GameObject prefabToSpawn2;
    public GameObject prefabToSpawn3;
    public float spawnAreaWidth = 100f; // Larghezza dell'area in cui verranno spawnati i prefabs
    public float spawnAreaLength = 100f; // Lunghezza dell'area in cui verranno spawnati i prefabs
    public int numerodaspawnare =10;
    public float originepianox;
    public  float originepianoz;
    public float lunghezzapiano;
    public float larghezzapiano;
    private static int oggettiNelPiano ;
    public List<GameObject> spazzature = new List<GameObject>();
    public Rigidbody rb ;
    public float upwardForce;
    public int a ;
    public TextMeshProUGUI testo;
    public float totalTime = 60f;
    public float totalTime2;// Il tempo totale del timer in secondi.
    private float currentTime;
    private float currentTime2;
    
    void Start()
    { 
        a=0;
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
        // Debug.Log(oggettiNelPiano);
        if(currentTime>0){
            testo.text = Mathf.Round(currentTime).ToString();
            currentTime -= Time.deltaTime;
        }
        if (currentTime2>0){
             currentTime2 -=Time.deltaTime;
        }
         //currentTime2 -=Time.deltaTime;
        
         /*if (oggettiNelPiano==numerodaspawnare||a==1){
             a=1;
            rb.AddForce(Vector3.up * upwardForce);
         }*/
         if (currentTime <= 0 && a==0)
        {
            currentTime = 0;
             rb.AddForce(Vector3.up * upwardForce);
            // Esegui azioni quando il timer scade.
            // Ad esempio, puoi terminare il gioco o attivare un'altra logica.
        }
        if (currentTime2<=0){

          foreach (GameObject spazzatura in spazzature)
            {
                Destroy(spazzatura);
            }
           a=1;
           currentTime2=0;
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
      
    
     
}






