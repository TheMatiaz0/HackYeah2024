using DG.Tweening;
using Rubin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume2D : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float timeDelay = 0.5f;

    private Ticker updateTicker;
    [SerializeField]
    private Transform listener;

    private void Awake()
    {
        updateTicker = TickerCreator.CreateNormalTime(timeDelay);
    }

    private void Update()
    {
        if (updateTicker.Push())
        {
            float delta = 1 / Vector2.Distance(transform.position, listener.position);
            audioSource.DOFade(delta, timeDelay);
        }
    }
}
