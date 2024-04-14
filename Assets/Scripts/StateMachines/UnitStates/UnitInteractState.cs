using UnityEngine;


public class UnitInteractState : UnitBaseState
{
    private float timer;
    private float interactRate;
    private bool inRange = false;
    public UnitInteractState(UnitStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{stateMachine.gameObject.name} entered Interact state");
        interactRate = 3f; // Base this on stats?
        timer = interactRate;
        if (stateMachine.Unit.Target == null)
            stateMachine.SwitchState(new UnitIdleState(stateMachine));


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
                stateMachine.Unit.SetCommand(targetPos, stateMachine.Unit.Target, stateMachine.Unit.Task);
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

    }

    private void Interact()
    {
        if (stateMachine.Unit.Target == null)
        {
            stateMachine.Unit.SetTask(Unit.Tasks.None);
            stateMachine.SwitchState(new UnitIdleState(stateMachine));
            return;
        }
        if (!stateMachine.Energy.ChangeEnergy(stateMachine.Unit.Target.EnergyCost))
        {
            stateMachine.Unit.RestCommand();
            return;
        }

        timer = interactRate;

        stateMachine.Unit.Target.Interact();
    }
}
