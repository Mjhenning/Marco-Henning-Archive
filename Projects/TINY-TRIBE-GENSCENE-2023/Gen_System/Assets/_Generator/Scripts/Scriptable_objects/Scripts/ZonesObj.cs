using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenObj", menuName = "Generation/ZonesGenSO", order = 2)]
public class ZonesObj : GenerationObjs
{
    [Header("Map Based Settings")]
    public float minHeight;  //minimum height at which prefabs should start spawning
    public float maxHeight; //maximum height at which prefabs should stop spawning

    [Header("Cast Settings")]
    public LayerMask MeshMask;
}
