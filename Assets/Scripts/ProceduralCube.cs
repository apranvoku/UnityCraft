using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    public int posX, posY, posZ;
    private void Awake()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        MakeCube(new Vector3(posX, posY, posZ));
        UpdateMesh();
        vertices.Clear();
        triangles.Clear();
    }

    void MakeCube(Vector3 cubePos)
    {
        for(int i = 0; i < 6; i++)
        {
            MakeFace(i, cubePos);
        }
        
    }

    void MakeFace(int dir, Vector3 facePos)
    {
        vertices.AddRange(CubeMeshData.faceVertices(dir, facePos));

        int vcount = vertices.Count;

        triangles.Add(vcount - 4);
        triangles.Add(vcount - 4 + 1);
        triangles.Add(vcount - 4 + 2);
        triangles.Add(vcount - 4);
        triangles.Add(vcount - 4 + 2);
        triangles.Add(vcount - 4 + 3);
    }
    // Update is called once per frame
    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
