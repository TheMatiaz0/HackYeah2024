using System;
using System.Collections;
using System.Collections.Generic;
using Rubin;
using UnityEngine;

public class EndMachineController : MonoBehaviour
{
    [SerializeField] private Transform tiles;
    [SerializeField] private WaveController wave;

    [SerializeField] private GameObject pressEPrefab;
    [SerializeField] private Transform canvas;

    private GameObject instantiatedText = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (instantiatedText != null) return;
        instantiatedText = Instantiate(pressEPrefab, canvas);
        instantiatedText.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (instantiatedText == null) return;
        Destroy(instantiatedText.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (instantiatedText != null)
            instantiatedText.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        
        if (!Input.GetKey(KeyCode.E))
            return;
        var children = tiles.GetComponentsInChildren<Collider2D>();
        foreach (var i in children)
            i.isTrigger = true;
        Destroy(wave.gameObject);
    }
}
