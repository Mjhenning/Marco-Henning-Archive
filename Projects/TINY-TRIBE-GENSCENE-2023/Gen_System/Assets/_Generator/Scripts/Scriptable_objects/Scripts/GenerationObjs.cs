using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationObjs : ScriptableObject {
    
    public List<GameObject> prefabs = new List<GameObject>();
    
    [Space]
    
    [Header("Prefab Variation Settings")]
    [Range(0, 1)] 
    public float rotateTowards_Normal; //used to rotate objects towards normals at which scale
}
