using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Tracks))]
//public class CustomTrackEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        Tracks tracks = (Tracks)target;
//        //if (GUILayout.Button("Create Tracks"))
//        //{
//        //    tracks.CreateTracks();
//        //}
//        //if (GUILayout.Button("Delete track"))
//        //{
//        //    tracks.DeleteTrack();
//        //}
//    }
//}
[CustomEditor(typeof(Platform))]
public class CustomPlaytestCapsule : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Platform platform = (Platform)target;
        if (GUILayout.Button("Move left"))
        {
            platform.MoveToDestinationA();
        }
        if (GUILayout.Button("Move right"))
        {
            platform.MoveToDestinationB();
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