using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    PlayerStats stats;
    private Rigidbody rb;

    private void Awake()
    {
        this.stats = player.GetComponent<PlayerStats>();
    }
     
}
