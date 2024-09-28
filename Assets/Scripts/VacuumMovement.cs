using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    [SerializeField] private VacuumPipeController vacuum;
    [SerializeField] private MaterialHelper materialHelper;
    [SerializeField] private float throwingDelay = 0.05f;
    [Header("Sounds")]
    [SerializeField] private AudioSource vacuumAudioSource;
    [SerializeField] private AudioClip vacuumEngineClip;

    private float currentThrowingDelay = 0f;
    
    void Update()
    {
        if(currentThrowingDelay > 0)
            currentThrowingDelay -= Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0))
        {
            vacuum.IsSucking = true;
            vacuumAudioSource.clip = vacuumEngineClip;
            vacuumAudioSource.loop = true;
            vacuumAudioSource.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            vacuum.IsSucking = false;
            vacuumAudioSource.Stop();
        }

        
        if (Input.GetMouseButton(1) && currentThrowingDelay <= 0)
        {
            vacuum.ThrowTrash();
            currentThrowingDelay = throwingDelay;
        }

        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            vacuum.CurrentSuckingMode = materialHelper.SuckingModes[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            vacuum.CurrentSuckingMode = materialHelper.SuckingModes[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            vacuum.CurrentSuckingMode = materialHelper.SuckingModes[2];
        
        vacuum.FollowMouse(transform.position);
    }
}
