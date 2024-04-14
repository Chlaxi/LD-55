using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Energy : MonoBehaviour
{
    [SerializeField]
    private int maxEnergy = 100;
    private int currentEnergy;

    public Healthbar healthbar;

    private void Awake()
    {
        healthbar = GetComponent<Healthbar>();
        //Get stats from ScriptableObject
        currentEnergy = maxEnergy;
    }

    void Start()
    {
        healthbar.ChangeHealth(maxEnergy / 100 * currentEnergy);
    }

    public bool ChangeEnergy(int energy)
    {
        bool isLoss = Mathf.Sign(energy) < 0 ? true : false;
        if (isLoss && !haveEnoughEnergy(energy) )
        {
            Debug.Log("Energy Not enough energy");
            return false;
        }
        currentEnergy += energy;
        if(currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;
        Debug.Log(isLoss ? $"lost {energy} energy" : $"gained {energy} energy");
        Debug.Log($"{currentEnergy}/{maxEnergy} energy");
        healthbar.ChangeHealth(maxEnergy/100*currentEnergy);
        return true;
    }

    private bool haveEnoughEnergy(int energy)
    {
        int rawResult = currentEnergy + energy;
        bool result = rawResult >= 0;
        return result;
    }

    public int GetEnergy()
    {
        return currentEnergy;
    }

    public int GetMaxEnergy()
    {
        return maxEnergy;
    }

    public bool IsAboveThreshold()
    {
        int threshold = Mathf.FloorToInt(maxEnergy / 100 * 30);
        return currentEnergy >= threshold;        
    }
}
