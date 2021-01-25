using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BusRouteManager : MonoBehaviour
{
    [Header("Geometric Setting")]
    [SerializeField] private Vector2 mapCenter;
    [SerializeField] private float mapScale;

    [Header("Bus Route")]
    public GameObject busRoutePrefab;
    [SerializeField] List<BusRoute> busRoutes;

    [Header("Data")]
    [SerializeField] string rawData;

    public void ReadData()
    {
        TextAsset dataset = AssetDatabase.LoadAssetAtPath<TextAsset>(rawData);
        string[] dataLines = dataset.text.Split('\n');

        for (int i = 1; i < dataLines.Length; i++)
        {
            Debug.Log(i + " | " + dataLines[i]);
            string[] data = dataLines[i].Split(',');
            int routeIndex = int.Parse(data[0].Trim());
            string chineseName = data[1].Trim();
            int vertexIndex = int.Parse(data[2].Trim());
            float distance = float.Parse(data[5].Trim());
            //float angle = float.Parse(data[6].Trim());
            float longitude = float.Parse(data[7].Trim());
            float latitude = float.Parse(data[8].Trim());

            RouteNode newNode = new RouteNode(vertexIndex, distance, longitude, latitude);

            // exist
            BusRoute br = busRoutes.Find(x => x.routeIndex == routeIndex);
            if (br != null)
            {
                br.AddNode(newNode);
            }
            else
            {
                BusRoute busRoute = Instantiate(busRoutePrefab, transform).GetComponent<BusRoute>();
                busRoute.Initialize(routeIndex, chineseName);
                busRoute.AddNode(newNode);
                busRoutes.Add(busRoute);
            }
        }

        // sort and draw
        busRoutes = busRoutes.OrderBy(x => x.routeIndex).ToList();
        for (int i = 0; i < busRoutes.Count; i++)
        {
            busRoutes[i].transform.SetSiblingIndex(i);
            busRoutes[i].DrawLine(mapCenter, mapScale);
            busRoutes[i].gameObject.SetActive(false);
        }
    }
    public void UpdateBusRoutePosition()
    {
        foreach (BusRoute busRoute in busRoutes)
        {
            busRoute.DrawLine(mapCenter, mapScale);
        }
    }
    public void DeleteBusRoutes()
    {
        foreach (BusRoute busRoute in busRoutes)
        {
            DestroyImmediate(busRoute.gameObject);
        }
        busRoutes.Clear();
    }
}
