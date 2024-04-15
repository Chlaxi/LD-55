using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveGain : MonoBehaviour
{
    [SerializeField]
    GameController.Resources resource;
    [SerializeField]
    private Vector2 quanitityRange;
    [SerializeField]
    private Vector2 gainRate;
    private float timer;

    private void Start()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        timer = Random.Range(gainRate.x, gainRate.y);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0)
            return;

        StartTimer();
        EmitParticle();
    }

    private void EmitParticle()
    {

        int qty = quanitityRange.x != quanitityRange.y ?
            Mathf.FloorToInt(quanitityRange.x) :
            Mathf.FloorToInt(Random.Range(quanitityRange.x, quanitityRange.y));

        Debug.Log($"Pew! {qty} {resource} gained");
        GameController.Instance.AddResource(resource, qty);
    }
}
