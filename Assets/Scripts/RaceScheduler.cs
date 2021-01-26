using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class RaceScheduler : MonoBehaviour
{
    [SerializeField] List<Vector3> wayPoints = new List<Vector3>();
    [SerializeField] Vector3 currentWaypoint;
    [SerializeField] int currentWayPointIndex = 0;
    [SerializeField] int totalWayPointCount = 0;
    [SerializeField] float courseTime = 0f;
    [SerializeField] List<Transform> testCourse = new List<Transform>();
    [SerializeField] GameObject wayPointIndicator = null;
    // Start is called before the first frame update
    void Awake()
    {
        currentWaypoint = Vector3.negativeInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaypoint.Equals(Vector3.negativeInfinity))
            return;
        courseTime += Time.deltaTime;
        if (Physics.OverlapSphere(currentWaypoint, 120f, LayerMask.GetMask("Player")).Length > 0)
        {
            if (currentWayPointIndex < totalWayPointCount)
            {
                currentWaypoint = wayPoints[currentWayPointIndex + 1];
                currentWayPointIndex += 1;
                if (wayPointIndicator != null)
                    wayPointIndicator.transform.position = currentWaypoint;
            }
            else
            {
                CourseComplete();
            }
        }
    }
    void OnDrawGizmos()
    {
        if (currentWaypoint == Vector3.negativeInfinity || wayPoints.Count == 0)
            return;
        Vector3 lastWayPoint = wayPoints[0];
        foreach (Vector3 wayPoint in wayPoints)
        {
            if (wayPoint.Equals(currentWaypoint))
                Gizmos.color = new Color(1, 1, 0, 0.5f);
            else
                Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(wayPoint, 120f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wayPoint, lastWayPoint);
            lastWayPoint = wayPoint;
        }
    }

    void SetWayPointList(List<Vector3> list)
    {
        wayPoints = list;
        InitializeCourse();
    }

    void CourseComplete()
    {
        if (wayPointIndicator != null)
            wayPointIndicator.SetActive(false);
        currentWaypoint = Vector3.negativeInfinity;
        currentWayPointIndex = 0;
        totalWayPointCount = 0;
        wayPoints = new List<Vector3>();
    }
    [ExposeMethodInEditor]
    void ActivateTestCourse()
    {
        wayPoints = new List<Vector3>();
        foreach (Transform point in testCourse)
        {
            wayPoints.Add(point.position);
        }
        InitializeCourse();
    }

    void InitializeCourse()
    {
        Bus.Instance.rb.transform.position = wayPoints[0] + Vector3.up * 25f;
        Bus.Instance.transform.rotation = Quaternion.LookRotation(wayPoints[1] - wayPoints[0], Vector3.up);
        currentWayPointIndex = 1;
        totalWayPointCount = wayPoints.Count - 1;
        currentWaypoint = wayPoints[1];
        courseTime = 0f;
        if (wayPointIndicator != null)
        {
            wayPointIndicator.SetActive(true);
            wayPointIndicator.transform.position = currentWaypoint;
        }
    }
}
