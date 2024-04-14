using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class UnitMoveState : UnitBaseState
{
    private Vector2 currentWaypoint;
    public UnitMoveState(UnitStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{stateMachine.gameObject.name} entered Move state");
        currentWaypoint = stateMachine.Unit.GetCurrentWaypoint();

    }

    public override void FixedTick(float deltaTime)
    {
        float sqrDist = Vector2.SqrMagnitude(currentWaypoint - stateMachine.Rigidbody2D.position);
        if (sqrDist < 0.025f)
        {
            Debug.Log("Destination reached");
            if (!stateMachine.Unit.NextWaypoint())
            {
                stateMachine.Movement.Stop();
                stateMachine.Rigidbody2D.position = currentWaypoint;
                //Change state to idle, rest, or interact, depending on task.
                switch(stateMachine.Unit.Task)
                {
                    case Unit.Tasks.Rest:
                        stateMachine.SwitchState(new UnitRestState(stateMachine));
                        break;
                    case Unit.Tasks.Gather:
                        stateMachine.SwitchState(new UnitInteractState(stateMachine));
                        break;
                    case Unit.Tasks.None:
                    default:
                        stateMachine.SwitchState(new UnitIdleState(stateMachine));
                        break;


                }
            }
            else
            {
                currentWaypoint = stateMachine.Unit.GetCurrentWaypoint();
            }
        }
        else // Recalculate direction to handle movements of target
        {
            Vector3 dir = currentWaypoint - stateMachine.Rigidbody2D.position;
            stateMachine.Movement.SetDirection(dir.normalized);
        }

    }

    public override void Tick(float deltaTime) {}

    public override void Exit()
    {
        Debug.Log($"{stateMachine.gameObject.name} leaving Move state");
    }

}
