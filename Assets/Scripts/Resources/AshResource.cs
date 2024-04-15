using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AshResource : Interactable
{
    FurnaceStorage furnaceStorage;

    [SerializeField]
    private int requiredWood = 1;
    [SerializeField]
    private float processTime;
    private float timer;

    private int ashes = 0;

    public bool StartFurnace(){
        if(IsBurning())
            return false;

        renderer2D.color = Color.red;
        timer = processTime;
        return true;
    }

    public override void Interact(UnitStats stats)
    {
        if (!IsActive())
            return;

        GameController.Instance.AddResource(resource, ashes);
        ashes = 0;
        // Remove ashes indicator
    }

    private void Update()
    {
        if (!IsBurning())
            return;

        timer -= Time.deltaTime;
        if(!IsBurning())
        {
            Debug.Log("Ashes done!");
            ashes++;
            Deactivate();
        }


    }

    public bool IsBurning()
    {
        return timer > 0;
    }

    // Ironically, IsActive here means it's done burning
    public override bool IsActive()
    {
        return ashes > 0;
    }

    protected override void Deactivate()
    {
        renderer2D.color = colour;
    }

    protected override void Respawn()
    {
        
    }
}
