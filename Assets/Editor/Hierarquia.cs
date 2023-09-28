using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Hierarquia 
{
   static Hierarquia()
    {
        EditorApplication.hierarchyWindowItemOnGUI += RenderObjects;
    }

    private static void RenderObjects(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null) return;

        if (gameObject.TryGetComponent<CustomHeaderObject>(out var customHeaderObject))
        {
             EditorGUI.DrawRect(selectionRect, customHeaderObject.backgroundColor);
             EditorGUI.LabelField(selectionRect, gameObject.name.ToUpper(), new GUIStyle()
             {
                 alignment = TextAnchor.MiddleCenter,
                 fontStyle = FontStyle.Bold,
                 normal = new GUIStyleState() { textColor = customHeaderObject.TextColor }
             });
        }
    }
}
