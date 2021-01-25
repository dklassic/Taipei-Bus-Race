#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public static class FindAndReplaceMaterialRecursively
{
    [MenuItem("Auto/Replace Material Recursively Visit Prefabs")]
    private static void FindAndReplaceMaterialInSelected()
    {
        Material setMat = Resources.Load("Buildings", typeof(Material)) as Material;
        // EditorUtility.CollectDeepHierarchy does not include inactive children
        var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
            .Select(t => t.gameObject);
        var prefabs = new HashSet<Object>();
        int matCount = 0;
        int goCount = 0;
        foreach (var go in deeperSelection)
        {
            Renderer rend = go.GetComponent<Renderer>();
            if (rend != null)
            {
                if (rend.sharedMaterials[0] != setMat)
                {
                    if (PrefabUtility.IsPartOfAnyPrefab(go))
                    {
                        RecursivePrefabSource(go, prefabs, ref matCount, ref goCount, ref setMat);
                        rend = go.GetComponent<Renderer>();
                        // only check the first one but should work
                        if (rend.sharedMaterials[0] == setMat || rend == null)
                            continue;
                    }

                    Undo.RegisterCompleteObjectUndo(go, "update materials");
                    SetMaterial(go, ref setMat);
                    matCount++;
                    goCount++;
                }
            }
        }

        Debug.Log($"Found and changed {matCount} materials from {goCount} GameObjects");
    }

    // Prefabs can both be nested or variants, so best way to clean all is to go through them all
    // rather than jumping straight to the original prefab source.
    private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int matCount,
        ref int goCount, ref Material setMat)
    {
        var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
        // Only visit if source is valid, and hasn't been visited before
        if (source == null || !prefabs.Add(source))
            return;

        // go deep before removing, to differantiate local overrides from missing in source
        RecursivePrefabSource(source, prefabs, ref matCount, ref goCount, ref setMat);

        Renderer rend = source.GetComponent<Renderer>();
        if (rend != null)
        {
            if (rend.sharedMaterials[0] != setMat)
            {
                Undo.RegisterCompleteObjectUndo(source, "update materials");
                SetMaterial(source, ref setMat);
                matCount++;
                goCount++;
            }
        }
    }

    private static void SetMaterial(GameObject go, ref Material setMat)
    {
        Renderer rend = go.GetComponent<Renderer>();
        var mats = new Material[rend.sharedMaterials.Length];
        for (var j = 0; j < rend.sharedMaterials.Length; j++)
        {
            mats[j] = setMat;
        }
        rend.sharedMaterials = mats;
    }
}
#endif