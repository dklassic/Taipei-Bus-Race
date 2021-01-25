using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField] bool isTriggered = false;
    [SerializeField] float replenishTime = 10f;
    Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void OnTriggerEnter()
    {
        if (isTriggered)
            return;
        isTriggered = true;
        Bus.Instance.AddNitro(10);

#if UNITY_EDITOR
        rend.material.SetColor("_BaseMap", Color.gray);
#else
                rend.sharedMaterial.SetColor("_BaseMap", Color.gray);
#endif
        Invoke("Replenish", replenishTime);
    }

    void Replenish()
    {
        isTriggered = false;
        #if UNITY_EDITOR
        rend.material.SetColor("_BaseMap", Color.blue);
#else
                rend.sharedMaterial.SetColor("_BaseMap", Color.blue);
#endif
    }

}
