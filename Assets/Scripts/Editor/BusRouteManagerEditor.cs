using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BusRouteManager))]
public class BusRouteManagerEditor : Editor
{
    BusRouteManager m_Target;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Target = (BusRouteManager)target;

        EditorGUILayout.Space();
        // button
        if (GUILayout.Button("Read Data"))
        {
            m_Target.ReadData();
        }

        // button
        if (GUILayout.Button("Update Center and Scale"))
        {
            m_Target.UpdateBusRoutePosition();
        }

        // button
        if (GUILayout.Button("Remove All Bus Route"))
        {
            m_Target.DeleteBusRoutes();
        }

    }
}
