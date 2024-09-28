using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private float waveSpeed = 1f;
    [SerializeField] private float waveAcceleration = .2f;

    public float currentSpeed { get; private set; }

    private void Awake()
    {
        currentSpeed = waveSpeed;
    }

    void Update()
    {
        transform.position += new Vector3(0, 1, 0) * (Time.deltaTime * currentSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Trash>()==null)
            return;
        
        Destroy(other.gameObject);
        currentSpeed += waveAcceleration;
    }
}
