using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class VacuumPipeController : MonoBehaviour
{
    [SerializeField] private Collider2D pipeCollider;
    [SerializeField] private MaterialHelper materialHelper;
    [SerializeField] private int maxCapacity = 100;
    [SerializeField] private Trash[] trashPrefabs;
    [SerializeField] private Transform spawningPoint;
    [SerializeField] private float throwingSpeed = 30f;
    [Header("Audio")]
    [SerializeField] private AudioSource vacuumAudioSource;
    [SerializeField] private AudioClip suckSound;
    [SerializeField] private AudioClip rejectSound;
    [SerializeField] private float cooldownSound = 0.5f;
    
    [FormerlySerializedAs("isSucking")] public bool IsSucking = false;

    [FormerlySerializedAs("CurrentSuckingModes")]
    public MaterialKind CurrentSuckingMode;
    private Trash previousTrash;
    private float requiredTimer;
    private string[] availableMaterials;
    private Dictionary<string, int> usedSpace;


    private void Awake()
    {
        CurrentSuckingMode = materialHelper.SuckingModes[0];
        usedSpace = new Dictionary<string, int>();
        availableMaterials = new string[materialHelper.SuckingModes.Length];
    }

    private void Start()
    {
        for (int i = 0; i < materialHelper.SuckingModes.Length; i++)
        {
            availableMaterials[i] = materialHelper.SuckingModes[i].ToString();
            usedSpace.Add(availableMaterials[i], 0);
        }
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
        if (trash == null || trash.kind != CurrentSuckingMode)
        {   if (previousTrash != trash && Time.time >= requiredTimer)
            {
                vacuumAudioSource.PlayOneShot(rejectSound);
                previousTrash = trash;
                requiredTimer = Time.time + cooldownSound;
            }
            return;
        }

        usedSpace[trash.kind.ToString()]++;
        SuckOutTrash(trash);
    }

    private void SuckOutTrash(Trash trashObject)
    {
        vacuumAudioSource.PlayOneShot(suckSound);
        Destroy(trashObject.gameObject);
    }

    public void ThrowTrash()
    {
        Trash trash = null;
        foreach (var i in trashPrefabs)
        {
            if (i.kind != CurrentSuckingMode)
                continue;
            if (usedSpace[i.kind.ToString()] <= 0)
                return;
            
            trash = Instantiate(i);
            usedSpace[i.kind.ToString()]--;
            break;
        }

        if (trash == null)
            return;
        
        trash.transform.position = spawningPoint.position; 
        trash.GetComponent<Rigidbody2D>().AddForce((spawningPoint.position-pipeCollider.transform.position)*throwingSpeed, ForceMode2D.Impulse);

        
    }
}
