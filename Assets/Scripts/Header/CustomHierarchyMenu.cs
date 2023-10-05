#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CustomHierarchyMenu : MonoBehaviour
{
    [MenuItem("GameObject/Create Custom Header")]
    static void CreateCustomHeader(MenuCommand menuCommand)
    {
        GameObject obj = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(obj, "Create Custom Header");

        GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
        obj.AddComponent<CustomHeaderObject>();
        Selection.activeObject = obj;
    }
}

#endif