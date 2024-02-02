using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenObj", menuName = "Generation/ZonesWithinZoneGenSO", order = 4)]
public class ZoneWithinZoneObj : GenerationObjs
{
    public Vector3 PlacementScale; //limited minimum scale the prefabs can spawn as

    public LayerMask MeshMask;
}
