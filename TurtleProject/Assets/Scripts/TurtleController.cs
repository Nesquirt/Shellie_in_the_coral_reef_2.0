using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    [SerializeField] private float acceleration = 3;
    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float speed; //Serializzato per testing, DO NOT TOUCH IN EDITOR
    [SerializeField] private float maxRotationSpeed = 45;
    [SerializeField] private Rigidbody rb;
    private Vector3 eulerRotationSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.eulerRotationSpeed = new Vector3(0, this.maxRotationSpeed, 0);
       
    }


    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // -------------------------------------------------------------------- //
        //calcolo della velocità frontale
        if(speed>=0 && speed<=maxSpeed)
            speed += acceleration * v * Time.deltaTime;
        if (v == 0)
            speed += acceleration * v * Time.deltaTime * 0.5f;
        if (speed < 0)
            speed = 0;
        if (speed > maxSpeed)
            speed = maxSpeed;

        //movimento frontale
        if(speed>0)
        {
            this.rb.MovePosition(this.rb.position + this.transform.forward * (speed*Time.deltaTime));
        }

        //rallentamento se non è premuta W
        

        // -------------------------------------------------------------------- //
        //calcolo della rotazione laterale
        float lateralRotationSpeed = 0;
        if(Mathf.Abs(lateralRotationSpeed) <= maxRotationSpeed && h != 0)
        {
            lateralRotationSpeed += acceleration * h * Time.deltaTime*5; //TODO: forse è meglio mettere un'accelerazione di rotazione separata
        }
        //Se non sono premuti tasti laterali, la rotazione rallenta fino a tornare dritti
        else if (h == 0 && lateralRotationSpeed != 0) 
        {
            if (lateralRotationSpeed > 0)
                lateralRotationSpeed -= acceleration * Time.deltaTime * 0.5f;
            else
                lateralRotationSpeed += acceleration * Time.deltaTime * 0.5f; 
        }
        //if (Mathf.Abs(lateralRotationSpeed) < 0.01)
        //    lateralRotationSpeed = 0;

        //rotazione laterale
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + lateralRotationSpeed*3f, -lateralRotationSpeed*100);

        // -------------------------------------------------------------------- //
        //calcolo della rotazione verticale
        float verticalRotationSpeed = 0;
        if (Mathf.Abs(verticalRotationSpeed) <= maxRotationSpeed && Input.GetKey(KeyCode.Space))
        {
            verticalRotationSpeed += acceleration * Time.deltaTime * 5; //TODO: forse è meglio mettere un'accelerazione di rotazione separata
        }
        else if (Mathf.Abs(verticalRotationSpeed) <= maxRotationSpeed && Input.GetKey(KeyCode.LeftControl))
        {
            verticalRotationSpeed -= acceleration * Time.deltaTime * 5;
        }
        //Se non sono premuti tasti verticali, la rotazione rallenta fino a tornare dritti
        if(!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl) && verticalRotationSpeed != 0)
        {
            if (verticalRotationSpeed > 0)
                verticalRotationSpeed -= acceleration * Time.deltaTime * 0.5f;
            else
                verticalRotationSpeed += acceleration * Time.deltaTime * 0.5f;
        }

        //rotazione verticale
        this.transform.eulerAngles = new Vector3(-verticalRotationSpeed * 100 , this.transform.eulerAngles.y, this.transform.eulerAngles.z);

        // -------------------------------------------------------------------- //

    }

    // -------------------------------------------------------------------- //
    //Funzione per eliminare la velocità generata da collisioni
    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("test");

        StartCoroutine(waiter());

        
    }
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //TODO: con Vector3.zero la velocità si taglia di colpo. Vedi se riesci a trovare un modo di rallentare in maniera graduale.
        //Intanto, per ora funziona.

        //this.speed = 0;
    }

    // -------------------------------------------------------------------- //
}
