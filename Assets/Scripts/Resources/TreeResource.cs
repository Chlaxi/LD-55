using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class TreeResource : Interactable
{

    private Animator animator;

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
        animator = GetComponent<Animator>();
        Respawn();
    }

    protected override void Respawn()
    {
        animator.SetBool("IsStump", false);
        int random = Mathf.CeilToInt(Random.Range(baseHealthModifier.x, baseHealthModifier.y));
        health = baseHealth + random;
        GenerateChipHealth();
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

        overflowDamage = stats.strength;
        health -= overflowDamage;

        do
        {
            if (!IsActive())
            {
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
        animator.SetBool("IsStump", true);
    }
}
