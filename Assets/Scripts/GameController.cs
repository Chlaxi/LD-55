using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Serializable]
    public struct ResourceImages
    {
        public Resources resource;
        public Sprite resourcesSprites;
    }

    public enum Resources { Wood, Ore, Fish, Ash, Wind, Crops, Lunar, Void }

    private static GameController instance = null;

    public static GameController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        resources.Add(Resources.Ash, 0);
        resources.Add(Resources.Fish, 0);
        resources.Add(Resources.Wood, 0);
        resources.Add(Resources.Ore, 0);
        resources.Add(Resources.Crops, 0);
        resources.Add(Resources.Lunar, 0);
        resources.Add(Resources.Void, 0);
        resources.Add(Resources.Wind, 0);
    }

    private void Start()
    {
        foreach (var resource in resources.Keys)
        {
            UpdateUI(resource);
        }
    }

    private Dictionary<Resources, int> resources = new Dictionary<Resources, int>();
    [SerializeField]
    private ResourceImages[] resourceImages;

    [SerializeField]
    private Text wood;
    [SerializeField]
    private Text ore;
    [SerializeField]
    private Text crops;
    [SerializeField]
    private Text fish;
    [SerializeField]
    private Text ash;
    [SerializeField]
    private Text wind;
    [SerializeField]
    private Text lunar;
    [SerializeField]
    private Text Void;


    public Dictionary<Resources, int> GetResources()
    {
        return resources;
    }

    public int GetResource(Resources resource)
    {
        return resources[resource];
    }

    public void AddResource(Resources resource, int value)
    {
        resources[resource] += value;
        UpdateUI(resource);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AddResource(Resources.Void, 1);

        if (Input.GetKeyDown(KeyCode.D))
            SpendResource(Resources.Void, 1);
    }

    public bool SpendResource(Resources resource, int value)
    {
        int currentAmount = resources[resource];
        if (currentAmount - value < 0 )
        {
            Debug.Log($"Not enough {resource} ({currentAmount})");
            return false;
        }
        resources[resource] -= value;
        UpdateUI(resource);
        return true;
    }

    [field: SerializeField]
    public Transform SummoningCircle { get; private set; }

    private void UpdateUI(Resources resource)
    {
        int amount = resources[resource];
        Text textElement = null;
        switch(resource) { 
            case Resources.Ash:
                textElement = ash; 
                break;
            case Resources.Wind:
                textElement = wind;
                break;
            case Resources.Wood:
                textElement = wood;
                break;
            case Resources.Ore:
                textElement = ore;
                break;
            case Resources.Crops:
                textElement = crops;
                break;
            case Resources.Void:
                textElement = Void;
                break;
            case Resources.Lunar:
                textElement = lunar;
                break;
            case Resources.Fish:
                textElement = fish;
                break;
        }

        textElement.text = amount.ToString();

    }
    public Sprite GetResourceSprite(Resources resource)
    {
        var res = resourceImages.Where(x => x.resource.Equals(resource)).First();
        return res.resourcesSprites;
    }
}
