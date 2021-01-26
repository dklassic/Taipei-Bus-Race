using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class BusRoute : MonoBehaviour
{
    [SerializeField] public int routeIndex { get; private set; }
    [SerializeField] public string routeName { get; private set; }
    [SerializeField] List<RouteNode> routeNodes;
    [SerializeField] List<Vector3> simplifiedRoutePosition;
    LineRenderer line = null;

    Vector2 center;
    float scale;

    [SerializeField] bool debug = true;

    public void Initialize(int index, string name, Vector2 center, float scale)
    {
        routeIndex = index;
        routeName = name;
        this.name = "Bus Route : " + index.ToString() + " | " + routeName;
        routeNodes = new List<RouteNode>();
        line = GetComponent<LineRenderer>();
        line.startWidth = 10;
        line.endWidth = 10;

        SetCenterAndScale(center, scale);
    }

    public void SetCenterAndScale(Vector2 center, float scale)
    {
        this.center = center;
        this.scale = scale;
    }

    public void AddNode(RouteNode node)
    {
        routeNodes.Add(node);
    }

    public void SimplyfyRoute()
    {
        int originalCount = line.positionCount;
        float threshold = 5f;
        line.Simplify(threshold);
        Vector3[] pos = new Vector3[line.positionCount];
        line.GetPositions(pos);
        simplifiedRoutePosition = new List<Vector3>(pos);
        Debug.Log("Simplify Route: " + this.name + " | " + originalCount + " -> " + line.positionCount);
    }

    public List<Vector3> GetRouteNodesPositions(bool simplified = false)
    {
        if (simplified)
        {
            SimplyfyRoute();
            return simplifiedRoutePosition;
        }
        else
        {
            return routeNodes.Select(x => x.GetLocalPosition(center, scale)).ToList();
        }  
    }

    public void DrawLine(bool simplified = false)
    {
        // sort
        routeNodes = routeNodes.OrderBy(x => x.index).ToList();
        //clear old node
        line.positionCount = routeNodes.Count;

        // add node to line
        for (int i = 0; i< routeNodes.Count; i++)
        {
            Vector3 position = routeNodes[i].GetLocalPosition(center, scale);
            line.SetPosition(i, position);
        }


        if (simplified)
        {
            SimplyfyRoute();
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (debug)
        {
            for(int i=0; i<line.positionCount; i++)
            {
                
                //Gizmos.DrawIcon(line.GetPosition(i), i.ToString());
                Gizmos.DrawWireSphere(transform.TransformPoint(line.GetPosition(i)), 10);
            }
        }
    }
}

[System.Serializable]
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

    public Vector3 GetLocalPosition(Vector2 center, float scale)
    {
        return new Vector3((longitude - center.x) * scale, 5, (latitude - center.y) * scale);
    }
}
