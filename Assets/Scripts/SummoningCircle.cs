using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircle : MonoBehaviour, IInteractible
{
    [SerializeField]
    private GameObject summonMenu;
    [SerializeField]
    private GameObject summons;

    public void Interact(UnitStats stats)
    {
        summonMenu.SetActive(true);
    }

    private bool CheckCost(CreatureStoreData.Price[] prices)
    {
        foreach (var price in prices)
        {
            Debug.Log($"Costs {price.cost} {price.resource}");
        }
        // implement resource checking


        return true;
    }

    public bool SpawnCreature(CreatureStoreData creature)
    {
        if(!CheckCost(creature.ResourceCost)) 
            return false;

        // Reduce resources

        Instantiate(creature.Prefab);

        return true;
    }
}
