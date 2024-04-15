using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingResource : Interactable
{

    [SerializeField]
    private Vector2 activeTime;
    private float activeTimer;

    [SerializeField]
    private Vector2 respawnTime;
    private float respawnTimer;

    private void Start()
    {
        Respawn();
    }
    protected override void Respawn()
    {
        activeTimer = Random.Range(activeTime.x, activeTime.y);
        renderer2D.color = colour;
    }

    private void Update()
    {
        if(!IsActive())
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                Respawn();
            }
        }
        else
        {
            activeTimer -= Time.deltaTime;

            if (activeTimer <= 0f)
            {
                Deactivate();
            }
        }
    }

    public override void Interact(UnitStats stats)
    {
        // TODO: Use dex somehow?
        // TODO: Make it chance based
        GameController.Instance.AddResource(resource, resourceYield);
    }

    public override bool IsActive()
    {
        return activeTimer > 0f;
    }
    protected override void Deactivate()
    {
        respawnTimer = Random.Range(respawnTime.x, respawnTime.y);
        Debug.Log($"Respawn timer set to {respawnTimer}");
        renderer2D.color = Color.gray;
    }
}
