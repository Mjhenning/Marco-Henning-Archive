using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ObjectType {
    Wood,
    Stone,
    Aluminum,
    Food,
    
    Townhall,
    Housing,
    Medical,
    
    PowerGeneration,
    Quarry,
    Mine,
    Forestry,
    GreenHouse,
    
    WoodStorage,
    AluminumStorage,
    RockStorage,
    PowerStorage,
    FoodStorage,
    
    AnimalWaypoints
}


//Goal of listsmanager is to store all the objects in scene in multiple lists and to add fun

public class ListsManager : MonoBehaviour {

    public static ListsManager instance;
    
    public List<GameObject> Trees = new List<GameObject> ();
    public List<GameObject> BerryBushes = new List<GameObject> ();
    public List<GameObject> Ore = new List<GameObject> ();
    public List<GameObject> Stones = new List<GameObject> ();

    public List<GameObject> TownHall = new List<GameObject> ();
    public List<GameObject> Housing = new List<GameObject> ();
    public List<GameObject> MedicalCentre = new List<GameObject> ();
    
    public List<GameObject> PowerGeneration = new List<GameObject> ();
    public List<GameObject> Quarries = new List<GameObject> ();
    public List<GameObject> Mines = new List<GameObject> ();
    public List<GameObject> Forestries = new List<GameObject> ();
    public List<GameObject> Greenhouses = new List<GameObject> ();

    public List<GameObject> WoodStorages = new List<GameObject> ();
    public List<GameObject> AluminumStorages = new List<GameObject> ();
    public List<GameObject> RockStorages = new List<GameObject> ();
    public List<GameObject> PowerStorages = new List<GameObject> ();
    public List<GameObject> FoodStorages = new List<GameObject> ();

    public List<GameObject> AnimalPoints = new List<GameObject> ();

    void Awake () {
        instance = this;
    }
    

    public void AddToList (GameObject obj, ObjectType type) {
        switch (type) {
            case ObjectType.Aluminum:
                Ore.Add (obj);
                break;
            case ObjectType.Wood:
                Trees.Add (obj);
                break;
            case ObjectType.Food:
                BerryBushes.Add (obj);
                break;
            case ObjectType.Stone:
                Stones.Add (obj);
                break;
            
                
            
            case ObjectType.Housing:
                Housing.Add (obj);
                break;
            case ObjectType.Townhall:
                TownHall.Add (obj);
                break;
            case ObjectType.Medical:
                MedicalCentre.Add (obj);
                break;
            
            
            case ObjectType.PowerGeneration:
                PowerGeneration.Add (obj);
                break;
            case ObjectType.Quarry:
                Quarries.Add (obj);
                break;
            case ObjectType.Mine:
                Mines.Add (obj);
                break;
            case ObjectType.Forestry:
                Forestries.Add (obj);
                break;
            case ObjectType.GreenHouse:
                Greenhouses.Add (obj);
                break;
            
            
            case ObjectType.WoodStorage:
                WoodStorages.Add (obj);
                break;
            case ObjectType.AluminumStorage:
                AluminumStorages.Add (obj);
                break;
            case ObjectType.RockStorage:
                RockStorages.Add (obj);
                break;
            case ObjectType.PowerStorage:
                PowerStorages.Add (obj);
                break;
            case ObjectType.FoodStorage:
                FoodStorages.Add (obj);
                break;
            
            case ObjectType.AnimalWaypoints:
                AnimalPoints.Add (obj);
                break;
        }
    }

    public GameObject GrabClosestObj (Vector3 currentpos,ObjectType type) { //grabs the closest gameobject with an assigned type based off of what is needed

        GameObject ClosestObj = null;
        
        switch (type) {
            case ObjectType.Aluminum:
                ClosestObj = FindClosestObject (Ore, currentpos);
                break;
            case ObjectType.Wood:
                ClosestObj = FindClosestObject (Trees, currentpos);
                break;
            case ObjectType.Food:
                ClosestObj = FindClosestObject (BerryBushes, currentpos);
                break;
            case ObjectType.Stone:
                ClosestObj = FindClosestObject (Stones, currentpos);
                break;
            
                
            
            case ObjectType.Housing:
                ClosestObj = FindClosestObject (Housing, currentpos);
                break;
            case ObjectType.Townhall:
                ClosestObj = FindClosestObject (TownHall, currentpos);
                break;
            case ObjectType.Medical:
                ClosestObj = FindClosestObject (MedicalCentre, currentpos);
                break;
            
            
            case ObjectType.PowerGeneration:
                ClosestObj = FindClosestObject (PowerGeneration, currentpos);
                break;
            case ObjectType.Quarry:
                ClosestObj = FindClosestObject (Quarries, currentpos);
                break;
            case ObjectType.Mine:
                ClosestObj = FindClosestObject (Mines, currentpos);
                break;
            case ObjectType.Forestry:
                ClosestObj = FindClosestObject (Forestries, currentpos);
                break;
            case ObjectType.GreenHouse:
                ClosestObj = FindClosestObject (Greenhouses, currentpos);
                break;
            
            
            case ObjectType.WoodStorage:
                ClosestObj = FindClosestObject (WoodStorages, currentpos);
                break;
            case ObjectType.AluminumStorage:
                ClosestObj = FindClosestObject (AluminumStorages, currentpos);
                break;
            case ObjectType.RockStorage:
                ClosestObj = FindClosestObject (RockStorages, currentpos);
                break;
            case ObjectType.PowerStorage:
                ClosestObj = FindClosestObject (PowerStorages, currentpos);
                break;
            case ObjectType.FoodStorage:
                ClosestObj = FindClosestObject (FoodStorages, currentpos);
                break;
            
            case ObjectType.AnimalWaypoints:
                ClosestObj = FindClosestObject (AnimalPoints, currentpos);
                break;
        }
        return ClosestObj;
    }
    
    public GameObject GetRandomGameObject(ObjectType type)
    {
               GameObject RandomObj = null;
        
        switch (type) {
            case ObjectType.Aluminum:
                RandomObj = FindRandomObj (Ore);
                break;
            case ObjectType.Wood:
                RandomObj = FindRandomObj (Trees);
                break;
            case ObjectType.Food:
                RandomObj = FindRandomObj (BerryBushes);
                break;
            case ObjectType.Stone:
                RandomObj = FindRandomObj (Stones);
                break;
            
                
            
            case ObjectType.Housing:
                RandomObj = FindRandomObj (Housing);
                break;
            case ObjectType.Townhall:
                RandomObj = FindRandomObj (TownHall);
                break;
            case ObjectType.Medical:
                RandomObj = FindRandomObj (MedicalCentre);
                break;
            
            
            case ObjectType.PowerGeneration:
                RandomObj = FindRandomObj (PowerGeneration);
                break;
            case ObjectType.Quarry:
                RandomObj = FindRandomObj (Quarries);
                break;
            case ObjectType.Mine:
                RandomObj = FindRandomObj (Mines);
                break;
            case ObjectType.Forestry:
                RandomObj = FindRandomObj (Forestries);
                break;
            case ObjectType.GreenHouse:
                RandomObj = FindRandomObj (Greenhouses);
                break;
            
            
            case ObjectType.WoodStorage:
                RandomObj = FindRandomObj (WoodStorages);
                break;
            case ObjectType.AluminumStorage:
                RandomObj = FindRandomObj (AluminumStorages);
                break;
            case ObjectType.RockStorage:
                RandomObj = FindRandomObj (RockStorages);
                break;
            case ObjectType.PowerStorage:
                RandomObj = FindRandomObj (PowerStorages);
                break;
            case ObjectType.FoodStorage:
                RandomObj = FindRandomObj (FoodStorages);
                break;
            
            case ObjectType.AnimalWaypoints:
                RandomObj = FindRandomObj (AnimalPoints);
                break;
        }
        return RandomObj;
    }

    GameObject FindRandomObj (List<GameObject> objects) {
        
        if (objects.Count > 0) {
            GameObject destinationObj;
            int randomInt = Random.Range(0, objects.Count);
            destinationObj = objects[randomInt];

            return destinationObj;
        }
        else
        {
            return null;
        }
    }

    GameObject FindClosestObject(List<GameObject> objects, Vector3 currentPosition) //grabs the closest object off of a list of objects
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);
            if (distance < closestDistance)
            {
                closestObject = obj;
                closestDistance = distance;
            }
        }
        
        return closestObject;
    }
    

    public void Clear () { //clears all the lists

        Trees.Clear ();
        BerryBushes.Clear ();
        Ore.Clear ();
        Stones.Clear ();

        TownHall.Clear ();
        Housing.Clear ();
        MedicalCentre.Clear ();

        PowerGeneration.Clear ();
        Quarries.Clear ();
        Mines.Clear ();
        Forestries.Clear ();
        Greenhouses.Clear ();

        WoodStorages.Clear ();
        AluminumStorages.Clear ();
        RockStorages.Clear ();
        PowerStorages.Clear ();
        FoodStorages.Clear ();

        AnimalPoints.Clear ();
    }
    
}
