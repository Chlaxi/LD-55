using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private enum States { Idling, Moving, Interacting }

    SpriteRenderer renderer2D;
    Rigidbody2D rg;
    private MoveVelocity movement;
    Queue<Vector2> path;
    Vector2 currentWaypoint = Vector2.zero;
    Color highlightColour;
    Unit target;
    [SerializeField] float interactRange;
    float interactRangeSqr;
    [SerializeField]
    private float interactSpeed;
    private float interactTimer;
    private States state;

    private void Awake()
    {
        renderer2D = GetComponent<SpriteRenderer>();
        movement = GetComponent<MoveVelocity>();
        rg = GetComponent<Rigidbody2D>();
        path = new Queue<Vector2>();
        state = States.Idling; 

        interactRangeSqr = interactRange * interactRange;

        switch (gameObject.tag)
        {
            case "Enemy":
                highlightColour = Color.red;
                break;
            case "Player":
                highlightColour = Color.green;
                break;
            default:
                highlightColour = Color.yellow;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 targetPos = target.rg.position;
            float targetSqrDist = Vector2.SqrMagnitude(targetPos - rg.position);
            if (targetSqrDist <= interactRangeSqr)
            {
                if (state.Equals(States.Interacting))
                {
                    if (interactTimer < 0)
                        Interact();

                    interactTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    state = States.Interacting;
                    movement.Stop();
                }
            }
            else
            {
                if (Vector2.SqrMagnitude(targetPos - currentWaypoint) > interactRangeSqr)
                {
                    Debug.Log("target moved away, recalculating path");
                    SetCommand(targetPos, target);
                }
            }
        }

        if (!state.Equals(States.Moving))
            return;

        float sqrDist = Vector2.SqrMagnitude(currentWaypoint - rg.position);
        if (sqrDist < 0.025f) {
            Debug.Log("Destination reached");
            if (!NextWaypoint())
            {
                movement.Stop();
                rg.position = currentWaypoint;
                state = States.Idling;
            }
        }
        else
        {
            Vector3 dir = currentWaypoint - rg.position;
            movement.SetDirection(dir.normalized);
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

    public void SetCommand(Vector3 targetPosition, Unit target)
    {
        SetCommand(targetPosition);
        this.target = target;
    }

    public void SetCommand(Vector3 targetPosition)
    {
        target = null;
        path.Clear();
        Debug.Log(($"Command given to {0} for destination {1}", gameObject.name, targetPosition));
        Vector2 targetVector = new Vector2(targetPosition.x, targetPosition.y);
        Pathfinding(targetVector);
        NextWaypoint();
    }
    
    private void Pathfinding(Vector2 targetPosition)
    {
        //TODO: Implement A*
        path.Enqueue(targetPosition);
    }

    private void Interact()
    {
        if (target == null)
        {
            Debug.Log("No target. Setting back to idle");
            state = States.Idling;
            return;
        }
        Debug.Log(($"Interacted with {0}", target.name));
        interactTimer = interactSpeed;
        //TODO: Damage target
    }
    private bool NextWaypoint()
    {
        Debug.Log(path.Count);
        if (path.Count > 0)
        {
            currentWaypoint = path.Dequeue();
            state = States.Moving;
            return true;
        }

        return false;
    }
}
