using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIValues : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private VacuumPipeController vacuumController;
    [SerializeField] private WaveController waveController;

    //vacuum
    private bool isVacuumOn;
    private MaterialKind currentVacuumMode;
    //TODO: pojemnosc odkurzacza
    //wave
    private float waveSpeed;
    
    void Update()
    {
        isVacuumOn = vacuumController.IsSucking;
        currentVacuumMode = vacuumController.CurrentSuckingModes;
        waveSpeed = waveController.currentSpeed;
    }
}
