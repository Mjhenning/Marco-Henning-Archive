using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to generate a noiseMap

public static class Noise
{
    public static float [,] GenerateNoiseMap (int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //Used to generate new noiseMaps based on seeds inserted ALSO allows one to remember a previous seed and use it
        System.Random prng = new System.Random (seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next (-100000, 100000) + offset.x; //used to limit range between -100000 and 100000 otherwise it will return the same value if outside those bounds
            float offsetY = prng.Next (-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2 (offsetX, offsetY);
        }
        
        //NoiseScale can't be equal or less than 0
        if (noiseScale <= 0) {
            noiseScale = 0.00001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f; //used to scale from the middle
        float halfHeight = mapHeight / 2f; //used to scale from the middle
        
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX =(x - halfWidth )/ noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 -1; // *2 -1 to allow noise to be in range from -1 - 1 instead of 0 - 1
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++) { //used to normalize noisemap values
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //InverseLerp returns value between 0 and 1
            }
        }

        return noiseMap;
    }
    
}
