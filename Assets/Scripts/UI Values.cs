using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Image image;

    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Text[] texts; 

    //vacuum
    private bool isVacuumOn;
    private MaterialKind currentVacuumMode;
    
    //wave
    private float waveSpeed;

    private void Start()
    {
        UpdateFirstFrame();
    }

    void Update()
    {
        UpdateFirstFrame();
    }

    private void UpdateFirstFrame()
    {
        image.sprite = vacuumController.CurrentSuckingMode.AssignedSymbol;
        isVacuumOn = vacuumController.IsSucking;
        currentVacuumMode = vacuumController.CurrentSuckingMode;
        waveSpeed = waveController.currentSpeed;
        var i = 0;
        foreach (var item in vacuumController.UsedSpace.Keys)
        {       
            images[i].sprite = item.AssignedSymbol;
            var count = vacuumController.UsedSpace[item];
            texts[i].text = $" {count.ToString()}/{vacuumController.MaxCapacity}";

            i++;
        }

        text.text = $"IsVacuumOn: {isVacuumOn}\nCurrentVacuumMode: {currentVacuumMode}\nWaveSpeed: {waveSpeed.ToString()}\n";
    }
}
