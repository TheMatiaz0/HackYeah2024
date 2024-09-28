using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuckingModes", menuName = "SuckingModes")]
public class MaterialHelper : ScriptableObject
{
    public MaterialKind[] SuckingModes;
    // {"Glass", "Paper", "Plastic"}
}