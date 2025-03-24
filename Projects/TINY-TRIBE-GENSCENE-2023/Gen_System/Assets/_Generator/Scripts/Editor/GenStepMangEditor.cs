using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Script used to add extensions onto editor 

[CustomEditor(typeof(GenerationStepManager))]
public class GenStepMangEditor : Editor {
    GenerationStepManager GenStepMang;

    void OnEnable () {
        GenStepMang = (GenerationStepManager) target;
    }

    public override void OnInspectorGUI () {

        DrawDefaultInspector ();


        if (GUILayout.Button("Generate")) {
            GenStepMang.Generate ();
        }
        
        if (GUILayout.Button("Clear")) {
            GenStepMang.Clear ();
        }
    }
}
