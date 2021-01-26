using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BusStopManager))]
public class BusStopManagerEditor : Editor
{
    BusStopManager m_Target;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Target = (BusStopManager)target;

        EditorGUILayout.Space();

        GUILayout.Label("Data Process", EditorStyles.boldLabel);
        // button
        if (GUILayout.Button("Read Data"))
        {
            m_Target.ReadData();
        }

        // button
        if (GUILayout.Button("Update Center and Scale"))
        {
            m_Target.UpdateBusStopPosition();
        }

        // button
        if (GUILayout.Button("Remove All Bus Stop"))
        {
            m_Target.DeleteBusStops();
        }
    }
}
