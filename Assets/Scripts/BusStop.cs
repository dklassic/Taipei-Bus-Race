using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] BusStopInfo info;
    [SerializeField] bool isTriggered = false;
    [SerializeField] float replenishTime = 10f;
    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void OnTriggerEnter()
    {
        if (isTriggered || Bus.Instance.IsNitroFull())
            return;
        rend.enabled = false;
        isTriggered = true;
        Bus.Instance.AddNitro(10);
        Invoke("Replenish", replenishTime);
    }

    void Replenish()
    {
        rend.enabled = true;
        isTriggered = false;
    }
}
