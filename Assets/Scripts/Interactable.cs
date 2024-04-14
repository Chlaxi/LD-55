using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Rigidbody2D rg {  get; private set; }
    protected SpriteRenderer renderer2D;
    protected Color colour;

    [SerializeField]
    private int energyCost = 2;

    [SerializeField, Range(1, 10)]
    protected int resourceYield = 1;

    [SerializeField]
    protected GameController.Resources resource;

    public int EnergyCost { get => energyCost*-1;}

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        renderer2D = GetComponent<SpriteRenderer>();
        colour = renderer2D.color;
    }

    protected abstract void Respawn();
    public abstract void Interact(UnitStats stats);
    public abstract bool IsActive();
    protected abstract void Deactivate();

}
