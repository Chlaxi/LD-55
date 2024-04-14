using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitStats : ScriptableObject
{
    public float movementSpeed = 3f;
    public float mineRate = 3f;
    public float fishRate = 3f;
    public float burnRate = 3f;
    public float harvestRate = 3f;
    public int strenght = 1;
    public int dexterity = 1;

    public bool canCollectLunar = false;
    public float lunarRate = 10f;
    public bool canCollectVoid = false;
    public float voidRate = 10f;


}
