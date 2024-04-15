using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine : StateMachine
{
    public Energy Energy { get; private set; }
    public Unit Unit { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    public MoveVelocity Movement { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Energy = GetComponent<Energy>();
        Unit = GetComponent<Unit>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Movement = GetComponent<MoveVelocity>();
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SwitchState(new UnitIdleState(this));
    }
}
