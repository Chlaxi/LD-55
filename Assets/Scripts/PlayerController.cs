using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
            ClearSelection();
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
            return;
        }

        Collider2D[] colliderArray = Physics2D.OverlapPointAll(mousePos);
        Vector3 targetPos = mousePos;
        Interactable target = null;

        foreach (Collider2D collider in colliderArray)
        {
            Debug.Log(collider);
            Interactable res = collider.GetComponent<Interactable>();
            if (res == null)
            {
                continue;
            }
            targetPos = res.transform.position;
            target = res;
            break;
        }

        targetPos.z = 0;

        foreach(Unit unit in units)
        {
            // TODO: Change flag to be specific based on command type
            // TODO: Set formation
            if (target != null)
            {
                if (target.tag == "SummoningCircle") {
                    unit.RestCommand();
                    continue;
                }
                unit.SetCommand(targetPos, target, Unit.Tasks.Gather);
            } else
            {
                unit.SetCommand(targetPos);
            }
        }
        SetCommandFlag(targetPos);
    }
     
    // Consider layermask
    private void SelectUnits(Vector3 mousePosition)
    {
            Collider2D[] colliderArray = Physics2D.OverlapAreaAll(startPos, mousePosition);

            foreach (Collider2D collider in colliderArray)
            {
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
                if (!isSelectingMultiple)
                    break;
            }

        isSelectingMultiple = false;
        DisableSelectionArea();
    }

    private void ClearSelection()
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
