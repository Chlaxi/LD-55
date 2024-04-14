using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitRestState : UnitBaseState
{
    private float timer;
    private float regenRate;
    public UnitRestState(UnitStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{stateMachine.gameObject.name} entered Rest state");
        regenRate = 3f; // Base this on stats?
        timer = regenRate; 
    }

    public override void FixedTick(float deltaTime) {}


    public override void Tick(float deltaTime)
    {
        timer -= deltaTime;

        if (timer > 0)
            return;

        stateMachine.Energy.ChangeEnergy(10);
        if (stateMachine.Energy.GetEnergy() == stateMachine.Energy.GetMaxEnergy())
        {
            stateMachine.SwitchState(new UnitIdleState(stateMachine));
            return;
        }

        timer = regenRate; // Base on stats?

    }

    public override void Exit()
    {
        Debug.Log($"{stateMachine.gameObject.name} leaving Rest state");
    }
}
