using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CropResource : Interactable
{
    [SerializeField]
    private List<Vector2> growthPhasesRandom;
    private int currentPhase;
    private bool needsWater = false;
    private float activeTimer;

    [SerializeField]
    private List<Sprite> cropSprites;

    [SerializeField]
    private SpriteRenderer soilRenderer;
    [SerializeField]
    private List<Sprite> soilSprites;

    private void Start()
    {
        Respawn();
    }

    private float SetStageTime()
    {
        if(currentPhase < 0 || currentPhase > growthPhasesRandom.Count-1) {
            Debug.LogError("CurrentPhase is out of bouds!");
            return 0;
        }
        Vector2 phase = growthPhasesRandom[currentPhase];
        activeTimer = Random.Range(phase.x, phase.y);
        Debug.Log($"Timer set to {activeTimer}");
        return activeTimer;
    }

    protected override void Respawn()
    {
        currentPhase = 0;
        needsWater = true;
        SetSprite();
    }

    private void Update()
    {
        if (IsActive())
            return;
            
        activeTimer -= Time.deltaTime;
        if(activeTimer <= 0)
        {
            currentPhase++;
            needsWater = true;
            SetSprite();
        }
        
    }

    public override void Interact(UnitStats stats)
    {
        if (!needsWater)
            return;

        if (currentPhase > growthPhasesRandom.Count - 1)
        {
            GameController.Instance.AddResource(resource, resourceYield);
            Respawn();
            return;
        }

        SetStageTime();
        needsWater = false;
        Debug.Log($"Is now in phase {currentPhase}");
        SetSprite();

    }

    // Can interact every time the crop needs water or can be harvested
    public override bool IsActive()
    {
        return needsWater;
    }

    protected override void Deactivate()
    {
       
        

    }

    private void SetSprite()
    {
        renderer2D.sprite = cropSprites[currentPhase];
        soilRenderer.sprite = soilSprites[needsWater ? 0 : 1];
    }
}
