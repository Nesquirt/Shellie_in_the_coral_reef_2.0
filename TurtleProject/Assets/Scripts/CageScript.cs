using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private bool _isLocked = true;
    private GameObject door;
    private Animator anim;

    void Start()
    {
        this.door = transform.GetChild(1).gameObject;
        this.anim = this.door.GetComponent<Animator>();

    }

    //se chiusa la apre, altrimenti non fa nulla
    public void OpenCage()
    {
        if(_isLocked)
        {
            this.anim.SetTrigger("openCage");
            this._isLocked = false;
        }
    }

    public bool isLocked
    {
        get { return _isLocked; }
        set { _isLocked = value; }
    }

}
