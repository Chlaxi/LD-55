using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitBaseState
{
    public UnitIdleState(UnitStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{stateMachine.gameObject.name} entered Idle state");
        //Check current task and set command.
    }

    public override void FixedTick(float deltaTime)
    {
        // Implement random roaming?
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        Debug.Log($"{stateMachine.gameObject.name} leaving Idle state");
    }
}
