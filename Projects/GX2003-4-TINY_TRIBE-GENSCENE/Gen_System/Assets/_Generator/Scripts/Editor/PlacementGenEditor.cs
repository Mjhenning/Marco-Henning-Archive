using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Script used to add extensions onto editor 

[CustomEditor(typeof(PlacementGenerator))]
public class PlacementGenEditor : Editor {
    PlacementGenerator placeGen;

    void OnEnable () {
        placeGen = (PlacementGenerator) target;
    }

    public override void OnInspectorGUI () {

        DrawDefaultInspector ();


        if (GUILayout.Button("Generate")) {
            placeGen.Generate ();
        }

        if (GUILayout.Button("Clear")) {
            placeGen.Clear ();

        }
    }
}
