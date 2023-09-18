using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private bool _isLocked = true;
    private GameObject door;
    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        this.door = transform.GetChild(0).gameObject;
        this.anim = this.door.GetComponent<Animator>();

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
        this.anim.SetTrigger("openDoor");
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
        this.rb = this.GetComponent<Rigidbody>();
        this.rb.AddForce(Vector3.up * 10);
        Debug.Log("saleeeee");
    }


}
