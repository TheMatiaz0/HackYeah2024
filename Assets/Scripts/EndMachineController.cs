using System;
using System.Collections;
using System.Collections.Generic;
using Rubin;
using UnityEngine;

public class EndMachineController : MonoBehaviour
{
    [SerializeField] private Transform tiles;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!Input.GetKey(KeyCode.E) )
            return;

        var children = tiles.GetComponentsInChildren<Collider2D>();
        foreach (var i in children)
            i.isTrigger = true;
    }
}
