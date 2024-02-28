using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private bool _isLocked = true;
    private GameObject door;
    private Animator animDoor;
    private Rigidbody rb;
    //private Animator animCage;

    void Awake()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.door = transform.GetChild(0).gameObject;  //primo figlio della gabbia
        this.animDoor = this.door.GetComponent<Animator>();
        //this.animCage = GetComponent<Animator>();
        
    }


    public void OpenCage()
    {
        if(_isLocked)
        {
            StartCoroutine(openCageAnimation());
            this._isLocked = false;
        }
    }

    IEnumerator openCageAnimation()
    {
        this.animDoor.SetTrigger("openDoor");
        yield return new WaitForSeconds(3);
        Destroy(transform.GetChild(1).gameObject);   //granchio = secondo figlio della gabbia
        StopCoroutine(openCageAnimation());
    }


    public bool isLocked
    {
        get { return _isLocked; }
        set { _isLocked = value; }
    }

    //TODO: fai partire con animazione
    public void GoUp()
    {
        StartCoroutine(CageAnimation());
    }

    IEnumerator CageAnimation ()
    {
        while (this.transform.position.y < 100f)
        {
            this.rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);  //gabbia spinta su 
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Gabbia distrutta");
        Destroy(this);
    }




}
