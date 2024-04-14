using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class TreeResource : Interactable
{

    [SerializeField]
    private Vector2 respawnTime;
    [SerializeField]
    private int baseHealth;
    [SerializeField] 
    private Vector2 baseHealthModifier;

    [SerializeField] 
    private Vector2 chipHealthRange;

    private int overflowDamage = 0;
//    [SerializeField]
    private int chipHealth;
//    [SerializeField]
    private int health;
    private float timer = 0;


    private void Start()
    {
        Respawn();
    }

    protected override void Respawn()
    {
        int random = Mathf.CeilToInt(Random.Range(baseHealthModifier.x, baseHealthModifier.y));
        health = baseHealth + random;
        GenerateChipHealth();
        renderer2D.color = colour;
    }
    
    private void GenerateChipHealth()
    {
        if (health <= 0)
            return;

        int random = Mathf.CeilToInt(Random.Range(chipHealthRange.x, chipHealthRange.y));
        chipHealth = Mathf.Min(random, health);
    }

    public override void Interact(UnitStats stats)
    {
        if (!IsActive())
            return;

        overflowDamage = stats.strenght;
        health -= overflowDamage;

        int n = 0;
        do
        {
            if (!IsActive())
            {
                Debug.Log("Tree ran out of health. Exiting loop");
                break;
            }

            n++;
            if(n > 20)
            {
                Debug.LogError("OVER 20 ITERATIONS!");
                break;
            }
            int damage = overflowDamage;
            overflowDamage -= chipHealth;
            chipHealth -= damage;
            if(chipHealth <= 0)
            {
                GameController.Instance.AddResource(GameController.Resources.Wood, 1);
                GenerateChipHealth();
            }
                
            
        } while (overflowDamage > 0);

        if (!IsActive())
        {
            Deactivate();
        }
    }

    private void Update()
    {
        if (IsActive())
            return;

        timer -= Time.deltaTime;
        if (timer < 0)
            Respawn();
    }

    public override bool IsActive()
    {
        return health > 0;
    }

    protected override void Deactivate()
    {
        timer = Random.Range(respawnTime.x, respawnTime.y);
        Debug.Log($"Respawn timer set to {timer}");
        renderer2D.color = Color.gray;
    }
}
