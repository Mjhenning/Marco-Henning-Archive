using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenObj", menuName = "Generation/ZoneMiddleGenSO", order = 5)]
public class ZoneMiddleObj : GenerationObjs
{
    public Vector2 rotationRange; //limited range of rotation
    public Vector3 PlacementScale; //limited minimum scale the prefabs can spawn as

    public LayerMask MeshMask;
}
