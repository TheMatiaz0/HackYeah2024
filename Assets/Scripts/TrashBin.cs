using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private MaterialKind materialKind;
    [Header("Limit")]
    [SerializeField] private bool isLimitUsed = false;
    [SerializeField] private int limit = 100;

    private int usedSpace = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Trash>()?.kind != materialKind || usedSpace >= limit)
            return;
        Destroy(other.gameObject);
        if (isLimitUsed) usedSpace++;
    }
}
