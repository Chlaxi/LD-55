using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreResource : Interactable
{
    [SerializeField]
    private int health;
    private int chipHealth;
    private int overflowDamage = 0;

    private void Start()
    {
        Respawn();
    }

    protected override void Respawn()
    {
        chipHealth = health - overflowDamage;
    }

    protected override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void Interact(UnitStats stats)
    {
        int damage = stats.strength;
        overflowDamage = damage;
        overflowDamage = Mathf.Max(overflowDamage - chipHealth, 0);
        chipHealth -= damage;
        if(chipHealth <= 0)
        {
            GameController.Instance.AddResource(resource, resourceYield);
            Respawn();
        }
    }

    public override bool IsActive()
    {
        return true;
    }
}
