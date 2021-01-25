using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class BusRoute : MonoBehaviour
{
    [SerializeField] public int routeIndex { get; private set; }
    [SerializeField] string routeName;
    [SerializeField] List<RouteNode> routeNodes;
    LineRenderer line = null;
   
    public void Initialize(int index, string name)
    {
        routeIndex = index;
        routeName = name;
        this.name = "Bus Route : " + index.ToString() + " | " + routeName;
        routeNodes = new List<RouteNode>();
        line = GetComponent<LineRenderer>();
        line.startWidth = 10;
        line.endWidth = 10;
    }

    public void AddNode(RouteNode node)
    {
        routeNodes.Add(node);
    }

    public void DrawLine(Vector2 center, float scale)
    {
        // sort
        routeNodes = routeNodes.OrderBy(x => x.index).ToList();
        //clear old node
        line.positionCount = 0;
        line.positionCount = routeNodes.Count;
        // add node to line
        for (int i = 0; i<routeNodes.Count; i++)
        {
            Vector3 position = new Vector3((routeNodes[i].longitude - center.x) * scale, 5, (routeNodes[i].latitude - center.y) * scale);
            Debug.Log(i + " " + position);
            line.SetPosition(i, position);
        }
    }
}


public class RouteNode
{
    [SerializeField] public int index { get; private set; }
    [SerializeField] public float distance { get; private set; }
    [SerializeField] public float longitude { get; private set; }
    [SerializeField] public float latitude { get; private set; }

    public RouteNode(int index, float distance, float longitude, float latitude)
    {
        this.index = index;
        this.distance = distance;
        this.longitude = longitude;
        this.latitude = latitude;
    }
}
