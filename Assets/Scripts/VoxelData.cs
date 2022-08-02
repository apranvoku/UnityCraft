using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData : MonoBehaviour
{
    public int[,,] data;

    public void Start()
    {
        data = new int[,,]
    {
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 },{ 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } },
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 },{ 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } },
        { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 },{ 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } },
        { { 0, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 },{ 1, 1, 1, 1, 1 }, { 0, 1, 1, 1, 0 } },
        { { 0, 0, 0, 0, 0 }, { 0, 0, 1, 1, 0 }, { 0, 1, 1, 1, 0 },{ 0, 0, 1, 1, 0 }, { 0, 0, 0, 0, 0 } },
        { { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 1, 1, 1, 0 },{ 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0 } }
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