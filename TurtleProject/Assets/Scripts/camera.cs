using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject target;
    private Vector3 offset;
  
    void Start()
    {
          this.offset = this.transform.position - this.target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.target.transform.position+this.offset ;
       
    }

}