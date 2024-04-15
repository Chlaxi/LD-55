using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum Tasks { None, Rest, Gather } // Might have to expand on gather

    private UnitStateMachine stateMachine;
    public Tasks Task { get; private set; } = Tasks.None;
    [field: SerializeField]
    public UnitStats Stats { get; private set; }

    private Healthbar healthbar;
    public SpriteRenderer renderer2D {get; private set; }
    Queue<Vector2> path;
    Vector2 currentWaypoint = Vector2.zero;
    Color highlightColour;
    public Interactable Target { get; private set; }
    public float InteractRangeSqr { get; private set; }

    private void Awake()
    {
        stateMachine = GetComponent<UnitStateMachine>();
        renderer2D = GetComponent<SpriteRenderer>();
        path = new Queue<Vector2>();

        InteractRangeSqr = Stats.interactionRange * Stats.interactionRange;

        switch (gameObject.tag)
        {
            case "Player":
                highlightColour = Color.green;
                break;
            default:
                highlightColour = Color.yellow;
                break;
        }
    }

    public void SelectUnit()
    {
        renderer2D.color = highlightColour;
    }

    public void DeselectUnit()
    {
        renderer2D.color = Color.white;
    }

    public void SetCommand(Vector3 targetPosition, Interactable target, Tasks task = Tasks.None)
    {
        SetCommand(targetPosition, task);
        Target = target;

        // Set Designated task based on task type
    }

    public void SetCommand(Vector3 targetPosition, Tasks task = Tasks.None)
    {
        Target = null;
        SetTask(task);
        path.Clear();
        Vector2 targetVector = new Vector2(targetPosition.x, targetPosition.y);
        Pathfinding(targetVector);
        NextWaypoint();
        stateMachine.SwitchState(new UnitMoveState(stateMachine));
    }
    
    public void RestCommand()
    {
        SetCommand(GameController.Instance.SummoningCircle.transform.position,  Unit.Tasks.Rest);
    }

    private void Pathfinding(Vector2 targetPosition)
    {
        //TODO: Implement A*
        path.Enqueue(targetPosition);
    }

    private void Interact()
    {
        Debug.Log($"Interacting with {gameObject.name}");
    }

    public bool NextWaypoint()
    {
        Debug.Log(path.Count);
        if (path.Count > 0)
        {
            currentWaypoint = path.Dequeue();
            return true;
        }

        return false;
    }

    public Vector2 GetCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public void SetTask(Tasks task)
    {
        Task = task;
    }
}
