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
    [SerializeField] private AudioSource constantSource;
    [SerializeField] private AudioClip suckSound;
    [SerializeField] private AudioClip rejectSound;
    [SerializeField] private AudioClip vacuumEngineClip;
    [SerializeField] private float cooldownSound = 0.5f;
    
    [SerializeField] private Transform vacuumPeak;
    [SerializeField] private float ignoreRadius;
    [SerializeField] private Transform ignoreCenter;
    
    
    [SerializeField] private float suckingForce=0.01f;
    
    
    
    public bool IsSucking { get; private set; } = false;

    [FormerlySerializedAs("CurrentSuckingModes")]
    public MaterialKind CurrentSuckingMode;
    public Dictionary<MaterialKind, int> UsedSpace;
    
    private Trash previousTrash;
    private float requiredTimer;
    private MaterialKind[] availableMaterials;

    private void Awake()
    {
        CurrentSuckingMode = materialHelper.SuckingModes[0];
        UsedSpace = new Dictionary<MaterialKind, int>();
        availableMaterials = new MaterialKind[materialHelper.SuckingModes.Length];
    }

    public void ChangeSucking(bool val)
    {
        if (val)
        {
            
            constantSource.clip = vacuumEngineClip;
            constantSource.loop = true;
            constantSource.Play();
        }
        else
        {
            constantSource.Stop();
        }
        this.IsSucking = val;
        
    } 

    private void Start()
    {
        for (int i = 0; i < materialHelper.SuckingModes.Length; i++)
        {
            availableMaterials[i] = materialHelper.SuckingModes[i];
            UsedSpace.Add(availableMaterials[i], 0);
        }
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
        if (IsSucking == false)
        {
            return;
        }
        
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

        UsedSpace[trash.kind]++;
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
            if (UsedSpace[i.kind] <= 0)
                return;
            
            trash = Instantiate(i);
            UsedSpace[i.kind]--;
            break;
        }

        if (trash == null)
            return;
        
        trash.transform.position = spawningPoint.position; 
        trash.GetComponent<Rigidbody2D>().AddForce((spawningPoint.position-pipeCollider.transform.position)*throwingSpeed, ForceMode2D.Impulse);

        
    }

    public void CallOnTriggerOnSuckPoint(GameObject itself,Collider2D other)
    {
        if (!this.IsSucking || !other.TryGetComponent(out Rigidbody2D rb2D))
            return;
        Trash trash = other.GetComponent<Trash>();
        if (trash == null)
        {
            return;
        }
        if(this.CurrentSuckingMode  == trash.kind)
            rb2D.AddForce((itself.transform.position - other.transform.position).normalized * suckingForce, ForceMode2D.Impulse);
    }
}
