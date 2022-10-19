using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData : MonoBehaviour
{
    public int[,,] data;

    public void Start()
    {
        data = new int[,,] 
    {           //Y1                 Y2                 Y3                Y4                 Y5                 Y6
         //Z1 Z2 Z3 Z4 Z5
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 },{ 0, 1, 1, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } }, //X0
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 },{ 1, 1, 1, 1, 1 }, { 0, 1, 1, 1, 0 }, { 0, 0, 1, 0, 0 } }, //X2
        { { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 1, 0, 0 },{ 1, 1, 1, 1, 1 }, { 0, 1, 1, 1, 0 }, { 0, 1, 1, 1, 0 } }, //X3
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 },{ 1, 1, 1, 1, 1 }, { 0, 1, 1, 1, 0 }, { 0, 0, 1, 0, 0 } }, //X4
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 },{ 0, 1, 1, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } }  //X5
    };
    }
    public int Width
    {
        get { return data.GetLength(0); }
    }

    public int Depth
    {
        get { return data.GetLength(1); }
    }

    public int GetCell(int x, int y, int z)
    {
        return data[x, y, z];
    }
}