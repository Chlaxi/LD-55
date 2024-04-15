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
#if UNITY_EDITOR
        stateMachine.Energy.healthbar.SetDebugStateText("Idling");
#endif
        stateMachine.Animator.SetTrigger("Idle");
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
        stateMachine.Animator.ResetTrigger("Idle");
    }
}
