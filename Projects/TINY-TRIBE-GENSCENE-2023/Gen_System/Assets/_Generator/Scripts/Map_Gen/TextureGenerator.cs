using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextFromColorMap (Color[] colorMap, int width, int height) { //generates a texture from a 1d array containing colors
        Texture2D text = new Texture2D (width, height);
        text.filterMode = FilterMode.Point;
        text.wrapMode = TextureWrapMode.Clamp;
        text.SetPixels (colorMap);
        text.Apply ();
        return text;
    }

    public static Texture2D TextFromHeightMap (float[,] heightMap) { //generates a texture from a heightmap (in our case noisemap) from a 2d array containing floats
        int width = heightMap.GetLength (0);
        int height = heightMap.GetLength (1);
        
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp (Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextFromColorMap (colorMap, width, height);
    }
}
