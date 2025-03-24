using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalloffGenerator : MonoBehaviour
{
    public static float[,] GenerateFallOffMap (Vector2Int size) {
        float[,] heightMap = new float[size.x, size.y];

        for (int x = 0; x < size.x; x++) { //creates a position vector based off of map sizes
            for (int y = 0; y < size.y; y++) {
                Vector2 pos = new Vector2 (
                    (float) x / size.x * 2 - 1,
                    (float) y / size.y * 2 - 1
                );
                
                //Find which value is closer to the edge;
                float value = Mathf.Max (Mathf.Abs (pos.x), Mathf.Abs (pos.y));

                //Used if you want to control faloff start and end (decided against it because it's not what I wanted)
                // if (value < falloffStart) {
                //     heightMap[x, y] = 1;
                // } else if (value > falloffEnd) {
                //     heightMap[x, y] = 0;
                // } else {
                //     heightMap[x, y] = value;
                //     
                //     //heightMap[x, y] = Mathf.SmoothStep (1, 0, Mathf.InverseLerp (falloffStart, falloffEnd, value)); //inverses faloffmap to force water
                // }
                
                heightMap[x, y] = Evaluate(value);
            }
        }

        return heightMap;
    }

    static float Evaluate (float value) { //put in place to gradually change faloff and to force it into a specific shape (put in place to help force map to have islands instead of 1 big landmass)
        float a = 2;
        float b = 3.2f;
        return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow(b - b * value,a));
    }
}
