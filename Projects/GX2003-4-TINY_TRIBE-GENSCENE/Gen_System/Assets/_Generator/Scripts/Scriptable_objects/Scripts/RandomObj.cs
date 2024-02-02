using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenObj", menuName = "Generation/RandomGenSO", order = 1)]
public class RandomObj : GenerationObjs
{
    public Vector2 rotationRange; //limited range of rotation
    public Vector3 minScale; //limited minimum scale the prefabs can spawn as
    public Vector3 maxScale; //limited maximum scale the prefabs can spawn as
    
    [Header("Map Based Settings")]
    public float minHeight;  //minimum height at which prefabs should start spawning
    public float maxHeight; //maximum height at which prefabs should stop spawning

    [Header("Cast Settings")]
    public LayerMask MeshMask;
}
