using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BusStopManager : MonoBehaviour
{
    [Header("Geometric Setting")]
    [SerializeField] private Vector2 mapCenter;
    [SerializeField] private float mapScale;

    [Header("Bus Stop")]
    public GameObject busStopPrefab;
    [SerializeField] List<BusStopInfo> busStops;

    [Header("Data")]
    [SerializeField] string rawData;

    public void ReadData()
    {
        TextAsset dataset = Resources.Load(rawData, typeof(TextAsset)) as TextAsset;
        string[] dataLines = dataset.text.Split('\n');

        for (int i = 1; i < dataLines.Length; i++)
        {
            Debug.Log(i + " | " + dataLines[i]);
            string[] data = dataLines[i].Split(',');
            int index = int.Parse(data[0].Trim());
            string chineseName = data[1].Trim();
            float longitude = float.Parse(data[2].Trim());
            float latitude = float.Parse(data[3].Trim());

            BusStopInfo busStop = Instantiate(busStopPrefab, transform).GetComponent<BusStopInfo>();
            busStop.Initialize(index, chineseName, longitude, latitude, mapCenter, mapScale);
            busStops.Add(busStop);
        }

        // sort
        busStops = busStops.OrderBy(x => x.GetIndex()).ToList();
        for(int i= 0 ; i<busStops.Count; i++)
        {
            busStops[i].transform.SetSiblingIndex(i);
        }
    }


    public void UpdateBusStopPosition()
    {
        foreach(BusStopInfo busStop in busStops)
        {
            busStop.UpdatePosition(mapCenter, mapScale);
        }
    }

    public void DeleteBusStops()
    {
        foreach (BusStopInfo busStop in busStops)
        {
            DestroyImmediate(busStop.gameObject);
        }
        busStops.Clear();
    }
}
