using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBaseState : State
{
    protected UnitStateMachine stateMachine;

    public UnitBaseState(UnitStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
