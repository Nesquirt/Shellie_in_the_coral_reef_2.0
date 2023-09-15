using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private bool _isLocked = true;
    [SerializeField] private GameObject door;
    //private Animator anim;

    void Start()
    {
        this.door = transform.GetChild(0).gameObject;
        //this.anim = door.GetComponent<Animator>();

    }

    //se � chiusa la apre, altrimenti non fa nulla
    public void OpenCage()
    {
        if(_isLocked)
        {
            //AGGIUNGERE animazione porta che si apre
            //this.anim.SetTrigger("open");
            this._isLocked = false;
        }
    }

    public bool isLocked
    {
        get { return _isLocked; }
        set { _isLocked = value; }
    }

}
