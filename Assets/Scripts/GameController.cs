using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [field: SerializeField]
    public NekonomiconUI nekonomicon { get; private set; } 

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
        nekonomicon.gameObject.SetActive(true); // Disables itself afterwards
        foreach (var resource in resources.Keys)
        {
            UpdateUI(resource);
        }
    }

    private Dictionary<Resources, int> resources = new Dictionary<Resources, int>();
    [SerializeField]
    private ResourceImages[] resourceImages;

    [SerializeField]
    private TextMeshProUGUI wood;
    [SerializeField]
    private TextMeshProUGUI ore;
    [SerializeField]
    private TextMeshProUGUI crops;
    [SerializeField]
    private TextMeshProUGUI fish;
    [SerializeField]
    private TextMeshProUGUI ash;
    [SerializeField]
    private TextMeshProUGUI wind;
    [SerializeField]
    private TextMeshProUGUI lunar;
    [SerializeField]
    private TextMeshProUGUI Void;


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
        if (Input.GetKeyUp(KeyCode.B))
        {
            nekonomicon.gameObject.SetActive(!nekonomicon.gameObject.activeSelf);
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {

            AddResource(Resources.Void, 1);
            AddResource(Resources.Wind, 1);
            AddResource(Resources.Wood, 1);
            AddResource(Resources.Fish, 1);
            AddResource(Resources.Crops, 1);
            AddResource(Resources.Lunar, 1);
            AddResource(Resources.Ore, 1);
            AddResource(Resources.Ash, 1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {

            SpendResource(Resources.Void, 1);
            SpendResource(Resources.Wind, 1);
            SpendResource(Resources.Wood, 1);
            SpendResource(Resources.Fish, 1);
            SpendResource(Resources.Crops, 1);
            SpendResource(Resources.Lunar, 1);
            SpendResource(Resources.Ore, 1);
            SpendResource(Resources.Ash, 1);
        }
#endif
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
    public SummoningCircle SummoningCircle { get; private set; }

    private void UpdateUI(Resources resource)
    {
        int amount = resources[resource];
        TextMeshProUGUI textElement = null;
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

    private bool CheckCost(CreatureStoreData.Price[] prices)
    {
        foreach (var price in prices)
        {
            if(GetResource(price.resource) < price.cost)
            {
                Debug.Log($"Not enough {price.resource}");
                return false;
            }
        }
        return true;
    }
    public bool SpawnCreature(CreatureStoreData creature)
    {
        if (!CheckCost(creature.ResourceCost))
            return false;

        foreach (var price in creature.ResourceCost)
        {
            SpendResource(price.resource, price.cost);
        }

        Instantiate(creature.Prefab, SummoningCircle.transform.position, Quaternion.identity);

        return true;
    }
}
