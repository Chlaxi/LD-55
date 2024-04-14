using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ResourceStats : ScriptableObject
{
    public GameController.Resources resourceType;
    public int baseYield = 1;
    public float harvestRate = 3f;
    public ResourceStats upgradePath;
}
