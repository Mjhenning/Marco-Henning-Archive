using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum PlacementMode {
    Random,
    Zones,
    Within_Zones,
    Zones_Within_Zone,
    ZoneMiddle
}

public enum GenMode {
    Null,
    Low,
    Medium,
    High
}

//Generatro used to place a prefab randomly across the map

public class PlacementGenerator : MonoBehaviour {

    public GenerationObjs genObj;
    public PlacementMode Mode;
    [SerializeField]GenMode DensityPreset = GenMode.Null;

    Vector2 xRange;
    Vector2 zRange;
    
    [Header ("Within Zone Placement Settings")]
    Vector3 center;
    float radius;
    SphereCollider sphere;

    [Header ("Raycast Settings")]
    public int AmountDuringNullToPlace;
    public int AmountToPlace; //amount of prefabs to spawn

    float Multiplier;
    
    public void Generate () { //spawns all the prefabs in random locations
        
        //Determine x and z range from mapgenerator's values
        xRange = new Vector2 (-(MapGenerator.instance.mapWidth * MapDisplay.instance.meshFilter.gameObject.transform.localScale.x), (MapGenerator.instance.mapWidth * MapDisplay.instance.meshFilter.gameObject.transform.localScale.x));
        
        zRange = new Vector2 (-(MapGenerator.instance.mapHeight * MapDisplay.instance.meshFilter.gameObject.transform.localScale.z), (MapGenerator.instance.mapHeight * MapDisplay.instance.meshFilter.gameObject.transform.localScale.z));
        
        //Grabs the chosen desnity preset off of the Generation Step Manager
        DensityPreset = GenerationStepManager.instance.DensityPreset;

        switch (DensityPreset) { //Determines a random multiplier
            case GenMode.Low:
               Multiplier = Random.Range(0.2f, 0.4f);
                break;
            case GenMode.Medium:
                Multiplier = Random.Range (0.4f, 0.7f);
                break;
            case GenMode.High:
                Multiplier = Random.Range (1.4f, 2f);
                break;
        }

        if (DensityPreset != GenMode.Null) { //if the density preset is not null calculate a new amount to place
            int NewAmount;
            NewAmount = (int) (AmountDuringNullToPlace * Multiplier);
            
            if (NewAmount != 0) { //always force it to atleast place 1
                AmountToPlace = NewAmount;
            } else {
                AmountToPlace = 1;
            }
        } else { //if the density preset is null grab the null to place amount
            AmountToPlace = AmountDuringNullToPlace;
        }

        switch (Mode) {
            case PlacementMode.Random: //used to randomly place the amount of prefab within a zone
                GenerateRandomly ();
                break;
            
            case PlacementMode.Zones: //used to generate zones
                GenerateZones ();
                break;
            
            case PlacementMode.Within_Zones: //used generate a bunch of the prefab within a given zone
                GenerateWithinZone ();
                break;
            
            case PlacementMode.Zones_Within_Zone: //used to generate prefab zones in zones
                GenerateZonesInZone ();
                break;
            
            case PlacementMode.ZoneMiddle: //used to generate in the middle of a zone
                GenerateInMiddleOfZone ();
                break;
        }
    }

    public void Clear () { //clear all the children that was spawned
        while (transform.childCount != 0) {
            DestroyImmediate (transform.GetChild (0).gameObject);
        }
    }

    public void CloseZones () {
        sphere.enabled = true;
    }
    
    void GenerateRandomly () {

        if (genObj is RandomObj obj) {
            for (int i = 0; i < AmountToPlace; i++)
            {
                bool isPlacementSuccessful = false;
                while (!isPlacementSuccessful) // Keep trying until a suitable location is found
                {
                    float samplex = Random.Range(xRange.x, xRange.y);
                    float sampleY = Random.Range(zRange.x, zRange.y);
                    Vector3 rayStart = new Vector3(samplex, obj.maxHeight, sampleY);

                    if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, obj.MeshMask))
                        continue;

                    if (hit.point.y < obj.minHeight)
                        continue;
                
                    Vector3 RandomizedScale = RandomizeScale (obj.minScale, obj.maxScale); //randomizes prefab scaled based off of scales assigned to scriptable
                
                    Collider[] colliders = Physics.OverlapBox(hit.point, new Vector3(RandomizedScale.x * 2f,RandomizedScale.y * 2f, RandomizedScale.z * 2f), quaternion.identity, ~obj.MeshMask); // Adjust the box size as per your prefab size
                
                    if (colliders.Length > 0)
                        continue; // There are colliders in the area, find a new position

                    GameObject randomPrefab = null;
                    
                    if (obj.prefabs.Count > 1) {
                        randomPrefab = obj.prefabs[Random.Range(0,obj.prefabs.Count)];
                    } else if (obj.prefabs.Count <=1) {
                        randomPrefab = obj.prefabs[0];
                    }
                    
                    GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(randomPrefab, transform);
                    
                    
                    instantiatedPrefab.transform.position = hit.point;
                    instantiatedPrefab.transform.Rotate(Vector3.up, Random.Range(obj.rotationRange.x, obj.rotationRange.y), Space.Self);
                    instantiatedPrefab.transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), obj.rotateTowards_Normal);
                    instantiatedPrefab.transform.localScale = RandomizedScale; 
                    
                    if (instantiatedPrefab.GetComponent<Object_Identifier>()) {
                        ListsManager listmanager = FindObjectOfType (typeof (ListsManager)) as ListsManager;
                        listmanager.AddToList (instantiatedPrefab, instantiatedPrefab.GetComponent<Object_Identifier> ().type);
                    }

                    isPlacementSuccessful = true; 
                }
            }

            GenerationStepManager.instance.IncRandomPopulationCount (); //increases populationcount to determine when "Done Generating" event fires
        }
    }
    
    void GenerateZones () {
        if (genObj is ZonesObj obj) {

            for (int i = 0; i < AmountToPlace; i++) {
                bool isPlacementSuccessful = false;
                while (!isPlacementSuccessful) {
                    float sampleX = Random.Range (xRange.x, xRange.y);
                    float sampleZ = Random.Range (zRange.x, zRange.y);
                    Vector3 rayStart = new Vector3 (sampleX, obj.maxHeight, sampleZ);

                    if (!Physics.Raycast (rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, obj.MeshMask))
                        continue;

                    if (hit.point.y < obj.minHeight)
                        continue;
                    
                    
                    GameObject randomPrefab = null;
                    
                    if (obj.prefabs.Count > 1) {
                        randomPrefab = obj.prefabs[Random.Range(0,obj.prefabs.Count)];
                    } else if (obj.prefabs.Count <=1) {
                        randomPrefab = obj.prefabs[0];
                    }
                    
                    // Use SphereCast to check for overlapping "Zone" objects
                    if (!Physics.SphereCast (hit.point, randomPrefab.GetComponent<SphereCollider> ().radius, Vector3.up, out RaycastHit sphereHit, 0f)) {
                        GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(randomPrefab, transform);
                        instantiatedPrefab.transform.position = hit.point;
                        instantiatedPrefab.transform.rotation = Quaternion.Lerp (transform.rotation, transform.rotation * Quaternion.FromToRotation (instantiatedPrefab.transform.up, hit.normal), obj.rotateTowards_Normal);

                        GenerationStepManager.instance.GenerateInZone.AddListener (instantiatedPrefab.GetComponent<PlacementGenerator> ().Generate); //add a listener to each zone to listen for the generateinzone event and if invoked fire the generate logic on this zone

                        if (instantiatedPrefab.GetComponent<PlacementGenTowns> ()) {
                            GenerationStepManager.instance.GenerateInZone.AddListener (instantiatedPrefab.GetComponent<PlacementGenTowns> ().Generate);
                        }

                        isPlacementSuccessful = true;
                    }
                }
            }

            GenerationStepManager.instance.ZoneCounter (); //Tell it to fire off the PlaceInZones script from main manager 
        }
    }
    

    void GenerateWithinZone () {
        if (genObj is WithinZoneObj obj) {
            
            sphere = GetComponent<SphereCollider> ();
            center = transform.position;
            radius = sphere.radius;
            sphere.enabled = false;


            for (int i = 0; i < AmountToPlace; i++) {
                bool isPlacementSuccessful = false;
                while (!isPlacementSuccessful) {
                    Vector3 randomPointInSphere = center + Random.insideUnitSphere * radius;
                    Vector3 rayStart = new Vector3 (randomPointInSphere.x, center.y + radius, randomPointInSphere.z);

                    if (!Physics.Raycast (rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, obj.MeshMask))
                        continue;

                    Vector3 RandomizedScale = RandomizeScale (obj.minScale, obj.maxScale);

                    Collider[] colliders = Physics.OverlapSphere (hit.point, 8, ~obj.MeshMask, QueryTriggerInteraction.Ignore); // Adjust the box size as per your prefab size

                    if (colliders.Length > 0)
                        continue; // There are colliders in the area, find a new position

                    GameObject randomPrefab = null;
                    
                    if (obj.prefabs.Count > 1) {
                        randomPrefab = obj.prefabs[Random.Range(0,obj.prefabs.Count)];
                    } else if (obj.prefabs.Count <=1) {
                        randomPrefab = obj.prefabs[0];
                    }
                    
                    GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(randomPrefab, transform);
                    instantiatedPrefab.transform.position = hit.point;
                    instantiatedPrefab.transform.Rotate (Vector3.up, Random.Range (obj.rotationRange.x, obj.rotationRange.y), Space.Self);
                    instantiatedPrefab.transform.rotation = Quaternion.Lerp (transform.rotation, transform.rotation * Quaternion.FromToRotation (instantiatedPrefab.transform.up, hit.normal), obj.rotateTowards_Normal);
                    instantiatedPrefab.transform.localScale = RandomizedScale;
                    

                     if (instantiatedPrefab.GetComponent<Object_Identifier>()) {
                        ListsManager listmanager = FindObjectOfType (typeof (ListsManager)) as ListsManager;
                         listmanager.AddToList (instantiatedPrefab, instantiatedPrefab.GetComponent<Object_Identifier> ().type);
                    }
                    
                    isPlacementSuccessful = true; // Set the flag to exit the loop
                }
            }

            GenerationStepManager.instance.GeneratedInZones.AddListener (CloseZones); //for each zone call void to close back up the sphere collider

            if (GetComponentInParent<PlacementGenTowns> ()) {
                GenerationStepManager.instance.InSubZoneCounter (GetComponentInParent<PlacementGenTowns> ().AmountToPlace); //if variant of script is found increase subzone counter
            } else {
                GenerationStepManager.instance.InZoneCounter (); //increases in zone counter
            }
        }
    }

    void GenerateZonesInZone () {
        if (genObj is ZoneWithinZoneObj obj) {
            sphere = GetComponent<SphereCollider> ();
            center = transform.position;
            radius = sphere.radius;
            sphere.enabled = false;


            for (int i = 0; i < AmountToPlace; i++) {
                bool isPlacementSuccessful = false;
                while (!isPlacementSuccessful) {
                    Vector3 randomPointInSphere = center + Random.insideUnitSphere * radius;
                    Vector3 rayStart = new Vector3 (randomPointInSphere.x, center.y + radius, randomPointInSphere.z);

                    if (!Physics.Raycast (rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, obj.MeshMask))
                        continue;

                    // Ensure the hit point is within the sphere collider
                    if (Vector3.Distance (hit.point, center) > (radius / 2))
                        continue;
                    

                    Collider[] colliders = Physics.OverlapSphere (hit.point, 4, ~obj.MeshMask, QueryTriggerInteraction.Ignore); // Adjust the box size as per your prefab size

                    if (colliders.Length > 0)
                        continue; // There are colliders in the area, find a new position

                    GameObject randomPrefab = null;
                    
                    if (obj.prefabs.Count > 1) {
                        randomPrefab = obj.prefabs[Random.Range(0,obj.prefabs.Count)];
                    } else if (obj.prefabs.Count <=1) {
                        randomPrefab = obj.prefabs[0];
                    }
                    
                    GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(randomPrefab, transform);
                    instantiatedPrefab.transform.position = hit.point;
                    instantiatedPrefab.transform.rotation = Quaternion.Lerp (transform.rotation, transform.rotation * Quaternion.FromToRotation (instantiatedPrefab.transform.up, hit.normal), obj.rotateTowards_Normal);
                    instantiatedPrefab.transform.localScale = obj.PlacementScale;

                    GenerationStepManager.instance.GeneratedSubZones.AddListener (instantiatedPrefab.GetComponent<PlacementGenerator> ().Generate);
                    
                    isPlacementSuccessful = true; // Set the flag to exit the loop
                }

                GenerationStepManager.instance.SubZoneCounter (AmountToPlace); //give it the amount of stuff to place in subzone and increase the counter subzone's own counter
            }
        }
    }

    void GenerateInMiddleOfZone () {
        if (genObj is ZoneMiddleObj obj) {
            
            sphere = GetComponent<SphereCollider> ();

            center = transform.position;
            radius = sphere.radius;
            sphere.enabled = false;


            for (int i = 0; i < AmountToPlace; i++) {
                bool isPlacementSuccessful = false;
                while (!isPlacementSuccessful) {
                    Vector3 rayStart = new Vector3 (center.x, center.y + radius, center.z);

                    if (!Physics.Raycast (rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, obj.MeshMask))
                        continue;
                    
                    GameObject randomPrefab = null;
                    
                    if (obj.prefabs.Count > 1) {
                        randomPrefab = obj.prefabs[Random.Range(0,obj.prefabs.Count)];
                    } else if (obj.prefabs.Count <=1) {
                        randomPrefab = obj.prefabs[0];
                    }
                    
                    GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(randomPrefab, transform);
                    
                    instantiatedPrefab.transform.position = hit.point;
                    instantiatedPrefab.transform.Rotate (Vector3.up, Random.Range (obj.rotationRange.x, obj.rotationRange.y), Space.Self);
                    instantiatedPrefab.transform.rotation = Quaternion.Lerp (transform.rotation, transform.rotation * Quaternion.FromToRotation (instantiatedPrefab.transform.up, hit.normal), obj.rotateTowards_Normal);
                    
                    instantiatedPrefab.transform.localScale = obj.PlacementScale;

                    if (instantiatedPrefab.GetComponent<Object_Identifier>()) {
                        ListsManager listmanager = FindObjectOfType (typeof (ListsManager)) as ListsManager;
                        listmanager.AddToList (instantiatedPrefab, instantiatedPrefab.GetComponent<Object_Identifier> ().type);
                    }

                    isPlacementSuccessful = true; // Set the flag to exit the loop
                }
            }

            GenerationStepManager.instance.GeneratedInZones.AddListener (CloseZones); //for each zone call void to close back up the sphere collider
            GenerationStepManager.instance.InZoneCounter (); //Increase the in zone counter
        }
    }

    Vector3 RandomizeScale (Vector3 minScale, Vector3 maxScale) { //randomizes scale based off of minimum vector 3 scale and maximum vector 3 scale
        return new Vector3 (
            Random.Range (minScale.x, maxScale.x),
            Random.Range (minScale.y, maxScale.y),
            Random.Range (minScale.z, maxScale.z)
        );
    }
    
}

