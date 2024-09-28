using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VacuumPipeController : MonoBehaviour
{
    [SerializeField] private Collider2D pipeCollider;
    [SerializeField] private MaterialHelper materialHelper;
    [SerializeField] private AudioSource vacuumAudioSource;
    [SerializeField] private AudioClip suckSound;
    [SerializeField] private AudioClip rejectSound;
    [SerializeField] private float cooldownSound = 0.5f;
    
    [SerializeField] private Transform vacuumPeak;
    [SerializeField] private float ignoreRadius;
    [SerializeField] private Transform ignoreCenter;
    
    
    
    [FormerlySerializedAs("isSucking")] public bool IsSucking = false;

    public MaterialKind CurrentSuckingModes;
    private Trash previousTrash;
    private float requiredTimer;

    private void Awake()
    {
        CurrentSuckingModes = materialHelper.SuckingModes[0];
    }

    private void OnDrawGizmos()
    {
        if (ignoreCenter != null)
            Gizmos.DrawWireSphere(ignoreCenter.transform.position, ignoreRadius);
    }


    public void FollowMouse(Vector2 parentPosition)
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mouseDir;
        float distane = Vector2.Distance(ignoreCenter.transform.position, mousePos);
        if (distane < ignoreRadius / 2)
        {
            return;
        }
        if (distane< ignoreRadius)
        {
            
             mouseDir = mousePos- (Vector2)this.ignoreCenter.transform.position;
        }
        else
        {
            
             mouseDir = mousePos- (Vector2)this.vacuumPeak.position;
        }
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        if (this.transform.lossyScale.x < 0)
        {
            angle += 180;
        }
        transform.rotation = (Quaternion.Euler(0,0, angle));
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var trash = other.GetComponent<Trash>();
        if (trash == null || trash.kind != CurrentSuckingModes)
        {   if (previousTrash != trash && Time.time >= requiredTimer)
            {
                vacuumAudioSource.PlayOneShot(rejectSound);
                previousTrash = trash;
                requiredTimer = Time.time + cooldownSound;
            }

            return;
        }


        SuckOutTrash(trash);
    }

    private void SuckOutTrash(Trash trashObject)
    {
        vacuumAudioSource.PlayOneShot(suckSound);
        Destroy(trashObject.gameObject);
    }
}
