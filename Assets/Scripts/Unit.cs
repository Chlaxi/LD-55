using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    SpriteRenderer renderer2D;
    Rigidbody2D rg;
    private MoveVelocity movement;
    Queue<Vector2> path;
    Vector2 currentWaypoint = Vector2.zero;
    Color highlightColour;
    bool isMoving = false;

    private void Awake()
    {
        renderer2D = GetComponent<SpriteRenderer>();
        movement = GetComponent<MoveVelocity>();
        rg = GetComponent<Rigidbody2D>();
        path = new Queue<Vector2>();

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
        if (!isMoving)
            return;

        float sqrDist = Vector2.SqrMagnitude(currentWaypoint - rg.position);
        if (sqrDist < 0.025f) {
            Debug.Log("Destination reached");
            if (!NextWaypoint())
            {
                movement.Stop();
                rg.position = currentWaypoint;
                isMoving = false;
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

    public void SetCommand(Vector3 targetPosition)
    {
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

    private bool NextWaypoint()
    {
        Debug.Log(path.Count);
        if (path.Count > 0)
        {
            currentWaypoint = path.Dequeue();
            isMoving = true;
            return true;
        }

        return false;
    }
}
