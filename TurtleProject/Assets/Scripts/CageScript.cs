using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private bool _isLocked = true;
    private GameObject door;
    private Animator animDoor;
    private Rigidbody rb;

    void Awake()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.door = transform.GetChild(0).gameObject;
        this.animDoor = this.door.GetComponent<Animator>();
        
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
        Destroy(transform.GetChild(1).gameObject); 
        StopCoroutine(openCageAnimation());
    }


    public bool isLocked
    {
        get { return _isLocked; }
        set { _isLocked = value; }
    }
    public void GoUp()
    {
        StartCoroutine(CageAnimation());
    }

    IEnumerator CageAnimation ()
    {
        while (this.transform.position.y < 100f)
        {
            this.rb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Gabbia distrutta");
        Destroy(this);
    }




}
