using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumSuckController : MonoBehaviour
{
    [SerializeField] private Collider2D suckCollider;
    [SerializeField] private VacuumPipeController vacuumController;
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!vacuumController.IsSucking || !other.TryGetComponent(out Rigidbody2D rb2D))
            return;
        rb2D.AddForce((transform.position - other.transform.position).normalized, ForceMode2D.Impulse);
    }
}
