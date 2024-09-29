using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMovement : MonoBehaviour
{
    private const string MouseScroll = "Mouse ScrollWheel";

    [SerializeField] private VacuumPipeController vacuum;
    [SerializeField] private MaterialHelper materialHelper;
    [SerializeField] private float throwingDelay = 0.05f;
    [Header("Sounds")]
    [SerializeField] private AudioSource vacuumAudioSource;
    [SerializeField] private AudioClip vacuumEngineClip;

    private int currentIndex;
    private float currentThrowingDelay = 0f;
    
    void Update()
    {
        if(currentThrowingDelay > 0)
            currentThrowingDelay -= Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0))
        {
            vacuum.ChangeSucking(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            vacuum.ChangeSucking(false);
        }

        
        if (Input.GetMouseButton(1) && currentThrowingDelay <= 0)
        {
            vacuum.ThrowTrash();
            currentThrowingDelay = throwingDelay;
        }

        if (Input.GetAxisRaw(MouseScroll) > 0)
        {
            currentIndex++;
        }
        else if (Input.GetAxisRaw(MouseScroll) < 0)
        {
            currentIndex--;
        }
        for (int i = 49; i < 52; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                vacuum.CurrentSuckingMode = materialHelper.SuckingModes[i - 49];
                currentIndex = i - 49;
            }
        }
        
        vacuum.FollowMouse(transform.position);
    }
}
