using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NekonomiconUI : MonoBehaviour
{ 
    [SerializeField]
    private GameObject creatureUIPrefab;

    [SerializeField]
    private List<CreatureStoreData> creatures;
    [SerializeField]
    GameObject content;

    private void Start()
    {
        foreach(CreatureStoreData creature in creatures)
        {
            GameObject creatureUI = Instantiate(creatureUIPrefab, content.transform);
            creatureUI.GetComponent<SummonCreatureUI>().SetData(creature);        
        }
        gameObject.SetActive(false);
    }
}
