using UnityEditor;
using UnityEngine;

public class CustomHeaderObject : MonoBehaviour
{
    public Color TextColor = Color.white;
    public Color backgroundColor = Color.red;

    private void OnValidate()
    {
        EditorApplication.RepaintHierarchyWindow();
    }
}
