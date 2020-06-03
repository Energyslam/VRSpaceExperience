using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Platform))]
public class CustomPlaytestCapsule : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Platform platform = (Platform)target;
        if (GUILayout.Button("Move left"))
        {
            platform.ChangeStateToA();
        }
        if (GUILayout.Button("Move right"))
        {
            platform.ChangeStateToB();
        }
        if (GUILayout.Button("Create Visuals"))
        {
            platform.CreateVisuals();
        }
        if (GUILayout.Button("Move to split"))
        {
            platform.ChangeStateToSplit();
        }
    }
}