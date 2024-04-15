using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningCircle : Interactable
{
    [SerializeField]
    private NekonomiconUI summonMenu;

    public override void Interact(UnitStats stats)
    {
        summonMenu.gameObject.SetActive(true);
    }

    protected override void Respawn() { }

    public override bool IsActive()
    {
        return true;
    }

    protected override void Deactivate() { }
}
