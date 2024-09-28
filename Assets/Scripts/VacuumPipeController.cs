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
    
    [FormerlySerializedAs("isSucking")] public bool IsSucking = false;

    public MaterialKind CurrentSuckingModes;
    private Trash previousTrash;
    private float requiredTimer;


    private void Awake()
    {
        CurrentSuckingModes = materialHelper.SuckingModes[0];
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
        if (trash == null || trash.kind != CurrentSuckingModes || IsSucking)
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
