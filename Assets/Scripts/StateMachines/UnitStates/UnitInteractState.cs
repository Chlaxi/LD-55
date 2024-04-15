using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class UnitInteractState : UnitBaseState
{
    private float timer;
    private float interactRate;
    private bool inRange = false;
    Interactable target;
    public UnitInteractState(UnitStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
#if UNITY_EDITOR
        stateMachine.Energy.healthbar.SetDebugStateText("Interacting");
#endif
        target = stateMachine.Unit.Target;
        interactRate = target.InstantInteraction ? 0.01f : stateMachine.Unit.Stats.interactionSpeed;
        timer = interactRate;
        if (target == null)
            stateMachine.SwitchState(new UnitIdleState(stateMachine));

        stateMachine.Animator.SetTrigger("InteractLoop");
    }

    public override void FixedTick(float deltaTime)
    {
        Vector2 targetPos = stateMachine.Unit.Target.rg.position;
        float targetSqrDist = Vector2.SqrMagnitude(targetPos - stateMachine.Rigidbody2D.position);
        inRange = targetSqrDist <= stateMachine.Unit.InteractRangeSqr;
        if (!inRange)
        {
            if (Vector2.SqrMagnitude(targetPos - stateMachine.Unit.GetCurrentWaypoint()) > stateMachine.Unit.InteractRangeSqr)
            {
                Debug.Log("target moved away, recalculating path");
                stateMachine.Unit.SetCommand(targetPos, target, stateMachine.Unit.Task);
            }
        }
    }

    public override void Tick(float deltaTime)
    {
        if (!inRange)
            return;

        timer -= deltaTime;

        if (timer > 0)
            return;

        // Check range and energy

        if (stateMachine.Energy.GetEnergy() <= 0)
        {
            Debug.Log("Energy depleted");
            stateMachine.Unit.RestCommand();
            return;
        }

        Interact();
    }

    public override void Exit()
    {
        stateMachine.Animator.ResetTrigger("Interact");
        stateMachine.Animator.ResetTrigger("InteractLoop");
    }

    private void Interact()
    {
        if (!target.IsActive())
        {
            Debug.Log("Target is not active. Switching to Idle state");
            return;
        }
        
            if (target == null)
        {
            stateMachine.Unit.SetTask(Unit.Tasks.None);
            stateMachine.SwitchState(new UnitIdleState(stateMachine));
            return;
        }
        if (!stateMachine.Energy.ChangeEnergy(target.EnergyCost))
        {
            stateMachine.Unit.RestCommand();
            return;
        }

        timer = interactRate;

        target.Interact(stateMachine.Unit.Stats);
        stateMachine.Animator.SetTrigger("Interact");

    }
}
