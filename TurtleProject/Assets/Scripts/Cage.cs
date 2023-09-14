using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] private bool _closed;

    void Start()
    {
        _closed = true;
    }


    public bool closed
    {
        get { return closed; }
        set { closed = value; }
    }

}
