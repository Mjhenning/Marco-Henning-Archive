using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenerationStepManager : MonoBehaviour {

    public static GenerationStepManager instance;
    public GenMode DensityPreset;

    [SerializeField] List<PlacementGenerator> Zones = new List<PlacementGenerator> ();
    public List<PlacementGenerator> RandomGens = new List<PlacementGenerator> ();

    public UnityEvent GenerateMesh = new UnityEvent ();
    public UnityEvent GenerateZones = new UnityEvent ();
    public UnityEvent GenerateFoilage = new UnityEvent ();
    [HideInInspector] public UnityEvent GenerateInZone = new UnityEvent ();
    [HideInInspector] public UnityEvent GeneratedInZones = new UnityEvent ();
    [HideInInspector] public UnityEvent GeneratedSubZones = new UnityEvent ();

    public UnityEvent Generation_Done = new UnityEvent ();

    int ZonesToFinishGeneratingIn;
    int zonesGeneratedIn;
    int ZonesGenerated;
    int SubZonesGenerated;
    int SubZoneGeneratedIn;
    int RandomZonesGenerated;

    void Awake () {
        instance = this;
    }

    public void Generate () {
        
        //Used to clear all data before generating again
        Clear ();
        SubZoneGeneratedIn = 0;
        SubZonesGenerated = 0;
        zonesGeneratedIn = 0;
        ZonesGenerated = 0;
        ZonesToFinishGeneratingIn = 0;
        RandomZonesGenerated = 0;
        
        
        //Dynamically sets up a target amount
        for (int i = 0; i < Zones.Count; i++) {
            ZonesToFinishGeneratingIn = ZonesToFinishGeneratingIn + Zones[i].AmountToPlace;
        }
        
        //called to generate
        GenerateMesh.Invoke ();
        
    }
    
    public void MeshGenerated () {
        //called to tell system mesh has been generated
        GenerateZones.Invoke ();
    }

    public void ZoneCounter () { //called to count zones and once all zones have been generated run logic to place stuff in zones
        ZonesGenerated++;

        if (ZonesGenerated == Zones.Count) {
            PlaceInZones ();
        }
    }
    public void InZoneCounter () { //everytime a zone's insides are placed a counter goes up and once all zones are placed invoke the event to tell system all insides of zones are done
        zonesGeneratedIn++;
        
        if (zonesGeneratedIn == ZonesToFinishGeneratingIn) {
            GeneratedInZones.Invoke ();
        }
    }
    public void SubZoneCounter (int ZoneAmount) { //increases in zone counter and when the amount is the same tell system when to start populating subzones
        SubZonesGenerated++;

        if (SubZonesGenerated == ZoneAmount ) {
            SubZonesCreated ();
        }
    }

    public void InSubZoneCounter (int SubZoneAmountDone) { //increases in sub zone count and when counter is the same as fed amount tell system to generate random foilages
        SubZoneGeneratedIn++;

        if (SubZoneGeneratedIn == SubZoneAmountDone) {
            GeneratedInZones.Invoke ();
        }
    }
    
    public void PlaceInZones () { //invokes event to tell systems to star placing in zones
        GenerateInZone.Invoke ();
    }

    public void SubZonesCreated () { //fired to  tell systems when to populate inside subzones are generated and when foilage can generate
        GeneratedSubZones.Invoke ();
        GenerateFoilage.Invoke ();
    }

    public void IncRandomPopulationCount () { //everytime a random generation is called increase a counter to determine when generation is done
        RandomZonesGenerated++;

        if (RandomZonesGenerated == RandomGens.Count) {
            AllGenerationDone ();
        }
    }

    IEnumerator WaitUntilEventTrigger () {
        yield return new WaitForSecondsRealtime (3);
        Generation_Done.Invoke ();
        Debug.Log ("All Generation has been finalised");
    }

    public void AllGenerationDone () { //fires off an event to allow next systems to fire off
        StartCoroutine ("WaitUntilEventTrigger");
    }
    
    public void Clear () { //used to clear everything
        ListsManager listmanager = FindObjectOfType (typeof (ListsManager)) as ListsManager;
        listmanager.Clear ();
        
        foreach (PlacementGenerator placegen in Zones) {
            placegen.Clear ();
        }

        foreach (PlacementGenerator placegen in RandomGens) {
            placegen.Clear ();
        }
        
    }
    
}
