using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    [SerializeField] private VacuumPipeController vacuum;
    
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            vacuum.IsSucking = true;
        if (Input.GetMouseButtonUp(0))
            vacuum.IsSucking = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            vacuum.CurrentSuckingMode = VacuumPipeController.ESuckingMode.Glass;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            vacuum.CurrentSuckingMode = VacuumPipeController.ESuckingMode.Paper;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            vacuum.CurrentSuckingMode = VacuumPipeController.ESuckingMode.Plastic;
        
        vacuum.FollowMouse(transform.position);
    }
}
