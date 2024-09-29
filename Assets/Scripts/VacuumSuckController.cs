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
        vacuumController.CallOnTriggerOnSuckPoint(this.gameObject,other);
    }
}
