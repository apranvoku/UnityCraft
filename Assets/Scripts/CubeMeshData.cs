using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeMeshData
{
    // Start is called before the first frame update
    public static Vector3[] vertices =
    {
        new Vector3( 1, 1, 1), // 0
        new Vector3( 0, 1, 1), // 1
        new Vector3( 0, 0, 1), // 2
        new Vector3( 1, 0, 1), // 3
        new Vector3( 0, 1, 0), // 4
        new Vector3( 1, 1, 0), // 5
        new Vector3( 1, 0, 0), // 6
        new Vector3( 0, 0, 0)  // 7
    };

    public static int[][] faceTriangles = {
    new int[]{0, 1, 2, 3 },//(1,1),(0,1),(0,0),(1,0) //North (x,y)
    new int[]{5, 0, 3, 6 },//(1,0),(1,1),(0,1),(0,0) //Right (y,z)
    new int[]{4, 5, 6, 7 },//(0,1),(1,1),(1,0),(0,0) //South (x,y)
    new int[]{1, 4, 7, 2 },//(1,1),(1,0),(0,0),(0,1) //Left  (y,z)
    new int[]{5, 4, 1, 0 },//(1,0),(0,0),(0,1),(1,1) //Top   (x,z)
    new int[]{3, 2, 7, 6 },//(1,1),(0,1),(0,0),(1,0) //Bottom(x,z)
    };

    public static Vector3[] faceVertices(int dir, Vector3 pos)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            fv[i] = pos + vertices[faceTriangles[dir][i]];
        }
        return fv;
    }
}
