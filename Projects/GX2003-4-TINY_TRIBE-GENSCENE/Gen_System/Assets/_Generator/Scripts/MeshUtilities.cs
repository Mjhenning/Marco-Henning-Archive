using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utilities script to determine the terrain type a specific y is at

public static class MeshUtilities
{
    public static Region GrabTerrainTypeBasedOnYPos (float y) {
        if (y > 3 && y <= 30) { //grass
            return Region.Grassland;
        }

        else if (y <= 1 && y >= 34f) { //beach
            return Region.Beach;
        }

        else if (y > 70) { //snow
            return Region.Snow;
        }

        else if (y >30 && y<=70) { //mountain
            return Region.Mountains;
        }

        else if (y < 3) {
            //ocean
            return Region.Ocean;
        } 
        else return Region.Null;
    }
}
