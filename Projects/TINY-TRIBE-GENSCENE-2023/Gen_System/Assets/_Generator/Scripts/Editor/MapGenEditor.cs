using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Script used to add extensions onto editor 

[CustomEditor(typeof(MapGenerator))]
public class MapGenEditor : Editor
{
    public override void OnInspectorGUI () {
        MapGenerator mapGen = (MapGenerator)target;

        CreateInspectorGUI ();
        
        if (DrawDefaultInspector()) { //if any value in inspector was changed
            if (mapGen.AutoUpdate) {
                mapGen.GenerateMap ();
            }
        }
        

        if (GUILayout.Button("Generate")) {
            mapGen.GenerateMap ();
        }
        
    }
}
