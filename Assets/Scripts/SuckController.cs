using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckController : MonoBehaviour
{
    [SerializeField] private Collider2D suckCollider;
    [SerializeField] private VacuumPipeController vacuumController;
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!vacuumController.IsSucking)
            return;

        other.GetComponent<Rigidbody2D>().AddForce((transform.position-other.transform.position).normalized, ForceMode2D.Impulse);           
    }
}
