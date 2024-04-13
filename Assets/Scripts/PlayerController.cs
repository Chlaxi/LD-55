using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private Vector3 startPos;
    List<Unit> targets;
    bool isSelectingMultiple = false;
    bool selectionIsHostile = false;
    
    void Start()
    {
        cam = Camera.main;
        targets = new List<Unit>();
    }

    // TODO: Draw selection rectangle
    void Update()
    {
        Vector3 currentMousePos = GetMouseWorldPos();

        if (Input.GetMouseButtonDown(0))
        {
            startPos = GetMouseWorldPos();
        }
        

        if (Input.GetMouseButton(0))
        {
            float sqrLen = Vector3.SqrMagnitude(currentMousePos - startPos);
            if(sqrLen > 0.1f)
                isSelectingMultiple = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ClearTargets();
            SelectUnits(currentMousePos);
        }
    }

    private void SelectUnits(Vector3 mousePosition)
    {
            selectionIsHostile = false;


            Collider2D[] colliderArray = Physics2D.OverlapAreaAll(startPos, mousePosition);



            foreach (Collider2D collider in colliderArray)
            {
                Debug.Log(collider);
                Unit unit = collider.GetComponent<Unit>();
                if (unit == null)
                {
                    continue;
                }

                if (isSelectingMultiple)
                {
                    if (!unit.gameObject.tag.Equals("Player"))
                        continue;
                }
                targets.Add(unit);
                unit.SelectUnit();
                selectionIsHostile = unit.gameObject.tag.Equals("Enemy");
                if (!isSelectingMultiple)
                    break;
            }
            isSelectingMultiple = false;
            Debug.Log(string.Format("Selected {0} objects", targets.Count));
            if (selectionIsHostile)
                Debug.Log("Selected hostile unit");
    }

    private void ClearTargets()
    {
        if (targets.IsUnityNull() || targets.Count < 1)
            return;

        foreach (Unit unit in targets)
        {
            unit.DeselectUnit();
        }

        targets.Clear();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cam.nearClipPlane));

        return point;
    }

    private void OnDrawGizmos()
    {
        if(Input.GetMouseButton(0) && isSelectingMultiple)
            Gizmos.DrawLine(startPos, GetMouseWorldPos());
    }
}
