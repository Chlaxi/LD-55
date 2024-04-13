using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    SpriteRenderer renderer;
    Color highlightColour;
    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
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

    public void SelectUnit()
    {
        renderer.color = highlightColour;
    }

    public void DeselectUnit()
    {
        renderer.color = Color.white;
    }

}
