using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    void FixedUpdate()
    {
        currentState?.FixedTick(Time.fixedDeltaTime);
    }

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}