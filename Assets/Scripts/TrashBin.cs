using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private MaterialKind materialKind;
    [Header("Limit")]
    [SerializeField] private bool isLimitUsed = false;
    [SerializeField] private int limit = 100;
    [SerializeField] private Text text;

    private int usedSpace = 0;

    private void Start()
    {
        text.text = "0";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Trash>()?.kind != materialKind || (isLimitUsed && usedSpace >= limit))
            return;
        Destroy(other.gameObject);
        usedSpace++;
        text.text = usedSpace.ToString();
    }
}
