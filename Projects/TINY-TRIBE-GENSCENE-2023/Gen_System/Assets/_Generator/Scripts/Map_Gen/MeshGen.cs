using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used to generate a terrain mesh

public static class MeshGen
{
    public static MeshData GenerateTerrainMesh (float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve) {
        int width = heightMap.GetLength (0);
        int height = heightMap.GetLength (1);
        float topLeftX = (width - 1) / -2f; //used to center mesh
        float topLeftZ = (height - 1) / 2f; //used to center mesh

        MeshData meshData = new MeshData (width, height);
        int vertexIndex = 0;
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                meshData.vertices[vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2 (x / (float) width, y / (float) height);

                if (x < width -1 && y < height -1) { //used to determine triangles (adds the 2 triangles of a quad between 4 vertices
                    meshData.AddTriangle (vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle (vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                
                vertexIndex++;
            }
            
        }

        return meshData;
    }
}


public class MeshData { //used to store and get all data from a mesh
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;
    public MeshData (int meshWidth, int meshHeight) { //used to calculate arrays of vertices, uvs and triangles
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle (int a, int b, int c) { //adds a triangle between specific vertices
        triangles[triangleIndex] = a;
        triangles[triangleIndex +1] = b;
        triangles[triangleIndex +2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh () { //used to create a mesh out of received info
        Mesh mesh = new Mesh ();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals (); //used to sort out lighting
        return mesh;
    }
}
