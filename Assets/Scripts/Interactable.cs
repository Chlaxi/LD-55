using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Rigidbody2D rg;

    [SerializeField]
    private int energyCost = 2;

    [SerializeField, Range(1, 10)]
    private int resourceYield = 1;

    [SerializeField]
    private GameController.Resources resource;

    public int EnergyCost { get => energyCost*-1;}

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    public void Interact()
    {
        GameController.Instance.AddResource(resource, resourceYield);
        Debug.Log($"Gained X resource");
    }

}
