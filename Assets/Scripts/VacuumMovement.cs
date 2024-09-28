using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    [SerializeField] private VacuumPipeController vacuum;
    [SerializeField] private MaterialHelper materialHelper;
    
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            vacuum.IsSucking = true;
        if (Input.GetMouseButtonUp(0))
            vacuum.IsSucking = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            vacuum.CurrentSuckingModes = materialHelper.SuckingModes[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            vacuum.CurrentSuckingModes = materialHelper.SuckingModes[1];
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            vacuum.CurrentSuckingModes = materialHelper.SuckingModes[2];
        
        vacuum.FollowMouse(transform.position);
    }
}
