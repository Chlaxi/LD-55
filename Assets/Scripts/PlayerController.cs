using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform selectionArea;
    [SerializeField]
    private Transform CommandFlag;

    private Camera cam;
    private Vector3 startPos;
    List<Unit> units;
    bool isSelectingMultiple = false;
    bool selectionIsHostile = false;
    
    void Awake()
    {
        cam = Camera.main;
        units = new List<Unit>();
        DisableSelectionArea();
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
            {
                isSelectingMultiple = true;
                // Draw selection area
                SetSelectionArea(currentMousePos);
            }
                

        }

        if (Input.GetMouseButtonUp(0))
        {
            ClearTargets();
            SelectUnits(currentMousePos);
        }

        if (Input.GetMouseButtonUp(1))
        {
            UnitCommand(currentMousePos);
        }
    }

    // Consider layermask
    private void UnitCommand(Vector3 mousePos)
    {
        if (units.Count < 1)
        {
            Debug.Log("No units selected");
            return;
        }

        if (selectionIsHostile)
        {
            Debug.Log("Can`t issue commmands to hostile units");
            return;
        }

        Collider2D[] colliderArray = Physics2D.OverlapPointAll(mousePos);
        Vector3 targetPos = mousePos;
        foreach (Collider2D collider in colliderArray)
        {
            Debug.Log(collider);
            Unit unit = collider.GetComponent<Unit>();
            if (unit == null)
            {
                continue;
            }

            targetPos = unit.transform.position;
            break;
        }

        targetPos.z = 0;

        foreach(Unit unit in units)
        {
            // TODO: Change flag to be specific based on command type
            // TODO: Set formation
            unit.SetCommand(targetPos);
        }
        SetCommandFlag(targetPos);
    }
     
    // Consider layermask
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
                units.Add(unit);
                unit.SelectUnit();
                selectionIsHostile = unit.gameObject.tag.Equals("Enemy");
                if (!isSelectingMultiple)
                    break;
            }

        isSelectingMultiple = false;
        DisableSelectionArea();
        Debug.Log(string.Format("Selected {0} objects", units.Count));
            if (selectionIsHostile)
                Debug.Log("Selected hostile unit");
    }

    private void ClearTargets()
    {
        if (units.IsUnityNull() || units.Count < 1)
            return;

        foreach (Unit unit in units)
        {
            unit.DeselectUnit();
        }

        units.Clear();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cam.nearClipPlane));

        return point;
    }

    private void SetCommandFlag(Vector3? flagPosition = null)
    {
        if (flagPosition == null)
        {
            CommandFlag.gameObject.SetActive(false);
            return;
        }

        CommandFlag.gameObject.SetActive(true);
        CommandFlag.position = flagPosition.Value;
        Debug.Log(($"Setting command position to {0}, {1}", flagPosition.Value.x, flagPosition.Value.y));
    }

    private void DisableSelectionArea()
    {
        selectionArea.gameObject.SetActive(false);
    }

    private void SetSelectionArea(Vector3 currentMousePos)
    {
        selectionArea.gameObject.SetActive(true);

        Vector3 selectionLowerLeft = new Vector3(
            Mathf.Min(startPos.x, currentMousePos.x),
            Mathf.Min(startPos.y, currentMousePos.y));
        Vector3 selectionTopRight = new Vector3(
            Mathf.Max(startPos.x, currentMousePos.x),
            Mathf.Max(startPos.y, currentMousePos.y));

        selectionArea.position = selectionLowerLeft;
        selectionArea.localScale = selectionTopRight - selectionLowerLeft;
    }
}
