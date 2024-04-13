using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Rigidbody2D rg;

    [SerializeField]
    private int energyCost = 2;

    public int EnergyCost { get => energyCost*-1;}

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    public void Interact()
    {
        //TODO: Perform interaction here
        Debug.Log($"Gained X resource");
    }

}
