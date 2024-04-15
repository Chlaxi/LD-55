using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceStorage : Interactable
{
    [SerializeField]
    AshResource[] furnaces;
    int woodStored = 5;
    [SerializeField]
    GameObject furnaceUI;
    public override void Interact(UnitStats stats)
    {
        furnaceUI.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RemoveWoodFromStorage();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AddWoodToStorage();
        }

        if (woodStored < 1)
            return;

        foreach (var f in furnaces)
        {

            if (f.StartFurnace())
                woodStored--;

            if (woodStored < 1)
                return;
        }
    }

    public override bool IsActive()
    {
        return true;
    }

    protected override void Deactivate() { }

    protected override void Respawn() { }

    public void AddWoodToStorage()
    {
        if (GameController.Instance.SpendResource(resource, 1))
            woodStored++;
    }

    public void RemoveWoodFromStorage()
    {
        if (woodStored > 0)
        {
            woodStored--;
            GameController.Instance.AddResource(resource, 1);
        }
    }
}
