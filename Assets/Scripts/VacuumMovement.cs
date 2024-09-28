using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    [SerializeField] private VacuumPipeController vacuum;
    [SerializeField] private MaterialHelper materialHelper;
    [SerializeField] private AudioSource vacuumAudioSource;
    [SerializeField] private AudioClip vacuumEngineClip;
    
    
    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
            vacuum.CurrentSuckingModes = materialHelper.SuckingModes[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            vacuum.CurrentSuckingModes = materialHelper.SuckingModes[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            vacuum.CurrentSuckingModes = materialHelper.SuckingModes[2];
        
        vacuum.FollowMouse(transform.position);
    }
}
