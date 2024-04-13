using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour
{
    Rigidbody2D rg;
    Vector3 direction = Vector3.zero;
    [SerializeField]
    float movementSpeed = 5f;

    void Awake()
    {
        rg = GetComponent<Rigidbody2D>();        
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void Stop()
    {
        direction = Vector3.zero;
    }

    void FixedUpdate()
    {
        rg.velocity = direction * movementSpeed;   
    }
}
