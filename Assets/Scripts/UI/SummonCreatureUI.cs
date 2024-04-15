using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonCreatureUI : MonoBehaviour
{
    public CreatureStoreData creatureData;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Transform resourcesFolder;

    [SerializeField]
    GameObject resourcePrefab;

    GameObject summonPrefab;

    public void SetData(CreatureStoreData creatureData)
    {
        this.creatureData = creatureData;
        image.sprite = creatureData.Potrait;
        description.text = creatureData.Description;
        foreach (var resource in creatureData.ResourceCost)
        {
            
            var resourceUI = Instantiate(resourcePrefab, resourcesFolder);
            resourceUI.GetComponentInChildren<Image>().sprite = GameController.Instance.GetResourceSprite(resource.resource);
            resourceUI.GetComponentInChildren<TextMeshProUGUI>().text= resource.cost.ToString();
        }
        summonPrefab = creatureData.Prefab;
    }

    public bool SummonCreature()
    {
        Debug.Log("Summoning creature!");
        return true;
    }
}
