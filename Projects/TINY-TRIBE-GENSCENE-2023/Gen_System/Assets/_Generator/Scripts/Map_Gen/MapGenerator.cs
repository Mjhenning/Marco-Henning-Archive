using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum Terrain_type {
    Deep_Water,
    Water_Shallow,
    Water,
    Sand,
    Grass,
    Grass_High,
    Stone,
    Rock,
    Snow
}

public enum Region {
    Ocean,
    Beach,
    Grassland,
    Mountains,
    Snow,
    Null
}

//Used to generate a worldmap

public class MapGenerator : MonoBehaviour {

    public static MapGenerator instance;
    
    
    public enum DrawMode {
        NoiseMap,
        ColorMap,
        Mesh,
        Falloff
    }

    public bool AutoUpdate;
    public DrawMode drawMode;
    
    [Header("Map Tweaks")]
    public int mapWidth;
    public int mapHeight;
    Vector2Int mapSize;
    [Range(0,100)]
    public float noiseScale;

    public int Octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    [Header("Falloff Settings")]
    public bool useFalloff;
    float[,] falloffMap;

    [Header("Map randomizers")]
    public int seed;
    public Vector2 offset;
    public float mapHeightMultiplier;
    
    [Header("Map Terrain Color Settings")]
    public AnimationCurve MeshHeightCurve; //used to determine how much each different terrain is affected by the multiplier
    public TerrainType[] Regions;

    void Awake () {
        //falloffMap = FalloffGenerator.GenerateFallOffMap (mapSize);
    }

    public void GenerateMap () { //generates a map by feeding given values

        instance = this;

        offset.x = Random.Range (0f, 10f);
        offset.y = Random.Range (0f, 10f);
        
        mapSize = new Vector2Int (mapWidth, mapHeight);
        seed = Random.Range (-10000, 10001);
        falloffMap = FalloffGenerator.GenerateFallOffMap (mapSize);
        
        float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, Octaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapWidth * mapHeight];
        
        for (int y = 0; y < mapHeight; y++) { //sets terrain colors
            for (int x = 0; x < mapWidth; x++) {
                if (useFalloff) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++) {
                    if (currentHeight <= Regions[i].height) {
                        colorMap[y * mapWidth + x] = Regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay _display = FindObjectOfType<MapDisplay> (); //displays this map on the object with a mapdisplay
        if (drawMode == DrawMode.NoiseMap) {
            _display.DrawTexture (TextureGenerator.TextFromHeightMap(noiseMap)); 
        } else if (drawMode == DrawMode.ColorMap) {
            _display.DrawTexture (TextureGenerator.TextFromColorMap (colorMap, mapWidth, mapHeight));
        } else if (drawMode == DrawMode.Mesh) {
            _display.DrawMesh (MeshGen.GenerateTerrainMesh (noiseMap, mapHeightMultiplier, MeshHeightCurve), TextureGenerator.TextFromColorMap (colorMap, mapWidth, mapHeight));
            _display.DrawNavMesh ();
        } else if (drawMode == DrawMode.Falloff) {
            _display.DrawTexture (TextureGenerator.TextFromHeightMap (FalloffGenerator.GenerateFallOffMap (mapSize)));
        }
    }

    void OnValidate () { //used to stop values from dropping beyond a certain threshold in editor
        if (mapWidth <1) {
            mapWidth = 1;
        }

        if (mapHeight < 1) {
            mapHeight = 1;
        }

        if (lacunarity <1) {
            lacunarity = 1;
        }

        if (Octaves <0) {
            Octaves = 0;
        }

        //falloffMap = FalloffGenerator.GenerateFallOffMap (mapSize);
    }
}

[System.Serializable] //Used to setup terrain types to generate colors on noisemap texture
public struct TerrainType {
    public Terrain_type Type;
    public float height;
    public Color color;
}
