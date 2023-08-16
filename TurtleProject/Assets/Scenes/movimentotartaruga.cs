using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimentotartaruga : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int speed=10;
     [SerializeField] private float speedrotation=90;
     [SerializeField] private Rigidbody rb;
     private Vector3 eulerRotationSpeed;
    float movimentoVerticale = 0f;
    // Start is called before the first frame update
    void Start()
    {
         this.rb=GetComponent<Rigidbody>();
         this.eulerRotationSpeed=new Vector3(0,this.speedrotation,0);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float h =Input.GetAxis("Horizontal");//mi restituisce un valore tra 0=non sta premendo nulla,1=eestra o d,-1=sinistra o a
        float v =Input.GetAxis("Vertical");//mi restituisve un valore 0 =nulla 1=è giù o s -1=è su o w
        //usiamo un vector3 perche modifichiamo il trasform che di tipo vector 3
        //this.rb.MoveRotation(this.rb.rotation-h*this.speedrotation*Time.deltaTime);
        //Quaternion deltaRotation = Quaternion.Euler(this.eulerRotationSpeed*(h*Time.deltaTime));
         //this.rb.MoveRotation(this.rb.rotation*deltaRotation);
        //this.rb.MovePosition(this.rb.position+this.transform.right*(v*this.speed*Time.deltaTime));
           
           

           if (Input.GetKey(KeyCode.A))
        {
        

          movimentoVerticale = 1f;
           //Quaternion deltaRotation = Quaternion.Euler(this.eulerRotationSpeed*(movimentoVerticale*Time.deltaTime));
         //this.rb.MoveRotation(this.rb.rotation*deltaRotation);
          Vector3 direzioneMovimento = new Vector3(movimentoVerticale, 0f, 0f);
         direzioneMovimento.Normalize(); 
        transform.Translate(direzioneMovimento * speed * Time.deltaTime);


        }

           if (Input.GetKey(KeyCode.D))
        {
        

          movimentoVerticale = -1f;
           //Quaternion deltaRotation = Quaternion.Euler(this.eulerRotationSpeed*(movimentoVerticale*Time.deltaTime));
         //this.rb.MoveRotation(this.rb.rotation*deltaRotation);
         Vector3 direzioneMovimento = new Vector3(movimentoVerticale, 0f, 0f);
         direzioneMovimento.Normalize(); 
        transform.Translate(direzioneMovimento * speed * Time.deltaTime);

          


        }
           
           if (Input.GetKey(KeyCode.W))
        {
        

          movimentoVerticale = -1f;
          Vector3 direzioneMovimento = new Vector3(0f, 0f, movimentoVerticale);
         direzioneMovimento.Normalize(); 
        transform.Translate(direzioneMovimento * speed * Time.deltaTime);


         }
         if (Input.GetKey(KeyCode.S))
        { 
        

          movimentoVerticale = +1f;
          Vector3 direzioneMovimento = new Vector3(0f, 0f, movimentoVerticale);
          direzioneMovimento.Normalize(); 
          transform.Translate(direzioneMovimento * speed * Time.deltaTime);


         }
           
           
            if (Input.GetKey(KeyCode.I))
        {
        

          movimentoVerticale = 1f;
          Vector3 direzioneMovimento = new Vector3(0f, movimentoVerticale, 0f);
          direzioneMovimento.Normalize(); 
         transform.Translate(direzioneMovimento * speed * Time.deltaTime);

    
         }
          if (Input.GetKey(KeyCode.O))
         {
          // Il tasto "W" è premuto.

          movimentoVerticale = -1f;
          Vector3 direzioneMovimento = new Vector3(0f, movimentoVerticale, 0f);
         direzioneMovimento.Normalize(); 
         transform.Translate(direzioneMovimento * speed * Time.deltaTime);

         }
   
    }
}
