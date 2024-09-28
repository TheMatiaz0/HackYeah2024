using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VacuumPipeController : MonoBehaviour
{
    [SerializeField] private Collider2D pipeCollider;
    [SerializeField] private MaterialHelper materialHelper;
    
    [FormerlySerializedAs("isSucking")] public bool IsSucking = false;

    public MaterialKind CurrentSuckingModes;


    private void Awake()
    {
        CurrentSuckingModes= materialHelper.SuckingModes[0];
    }

    public void FollowMouse(Vector2 parentPosition)
    {
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = parentPosition + mouseDir.normalized;
        transform.rotation = (Quaternion.Euler(0,0, Mathf.Atan2(mouseDir.y, mouseDir.x)*Mathf.Rad2Deg));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var trash = other.GetComponent<Trash>();
        if (trash == null || trash.kind != CurrentSuckingModes)
            return;
        
        Destroy(other.gameObject);
    }
}
