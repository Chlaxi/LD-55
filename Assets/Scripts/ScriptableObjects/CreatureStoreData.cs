using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[CreateAssetMenu]
public class CreatureStoreData : ScriptableObject
{
    [Serializable]
    public struct Price
    {
        public GameController.Resources resource;
        public int cost;
    }
    
        
    [field: SerializeField]
    public Sprite Potrait { get; private set; }
    [field: SerializeField]
    public string Description { get; private set; }
    [field: SerializeField]
    public Price[] ResourceCost { get; private set; }
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}
