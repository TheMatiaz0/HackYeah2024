using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndColliderController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("KONIEC");
    }
}
