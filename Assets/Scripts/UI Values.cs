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
    [SerializeField] private Text text;
    //vacuum
    private bool isVacuumOn;
    private MaterialKind currentVacuumMode;
    private Dictionary<string, int> usedSpace;
    
    //wave
    private float waveSpeed;
    
    void Update()
    {
        isVacuumOn = vacuumController.IsSucking;
        currentVacuumMode = vacuumController.CurrentSuckingMode;
        waveSpeed = waveController.currentSpeed;
        usedSpace = vacuumController.UsedSpace;

        text.text = $"IsVacuumOn: {isVacuumOn}\nCurrentVacuumMode: {currentVacuumMode}\nWaveSpeed: {waveSpeed.ToString()}\n";
        foreach (var i in usedSpace)
            text.text += i.ToString() + "\n";
    }
}
