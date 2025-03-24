using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenObj", menuName = "Generation/WithinZoneGenSO", order = 3)]
public class WithinZoneObj : GenerationObjs
{
    public Vector2 rotationRange; //limited range of rotation
    public Vector3 minScale; //limited minimum scale the prefabs can spawn as
    public Vector3 maxScale; //limited maximum scale the prefabs can spawn as

    public LayerMask MeshMask;
}
