using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BusStopInfo : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] string busStopName;
    [SerializeField] float longitude;
    [SerializeField] float latitude;

    public void Initialize(int index, string busStopName, float longitude, float latitude, Vector2 center, float scale)
    {
        this.index = index;
        this.busStopName = busStopName;
        this.longitude = longitude;
        this.latitude = latitude;

        UpdatePosition(center, scale);
        this.name = "Bus Stop : " + index.ToString() + " | " + busStopName;
    }

    public void UpdatePosition(Vector2 center, float scale)
    {
        transform.localPosition = new Vector3((longitude - center.x) * scale, 0, (latitude - center.y) * scale);
    }

    public int GetIndex() => index;
}
