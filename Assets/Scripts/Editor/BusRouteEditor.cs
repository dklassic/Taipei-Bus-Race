using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BusRoute))]
public class BusRouteEditor : Editor
{
    BusRoute m_Target;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Target = (BusRoute)target;

        EditorGUILayout.Space();
        // button
        if (GUILayout.Button("Simplyfy Route"))
        {
            m_Target.SimplyfyRoute();
        }

        // button
        if (GUILayout.Button("Draw Original Route"))
        {
            m_Target.DrawLine(false);
        }

        // button
        if (GUILayout.Button("Draw Simplyfed Route"))
        {
            m_Target.DrawLine(true);
        }
    }
}
