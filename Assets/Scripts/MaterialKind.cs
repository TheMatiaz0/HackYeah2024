using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MaterialKind", menuName = "MaterialKind")]
public class MaterialKind : ScriptableObject
{
    [SerializeField]
    private Sprite assignedSymbol;

    public Sprite AssignedSymbol => assignedSymbol;
}