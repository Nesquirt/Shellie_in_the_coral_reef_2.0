using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    private bool isLocked = true;
    [SerializeField] private GameObject door;

    void Start()
    {
        door = transform.GetChild(0).gameObject;
    }

    //se è chiusa la apre, altrimenti non fa nulla
    public void OpenCage()
    {
        if(isLocked)
        {
            //AGGIUNGERE animazione porta che si apre
            this.isLocked = false;
        }
    }

    //


    public bool closed
    {
        get { return closed; }
        set { closed = value; }
    }
    */

}
