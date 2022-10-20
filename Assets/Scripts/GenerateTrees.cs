using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrees : MonoBehaviour
{
    public int numTrees;
    public VoxelData vd;
    int layerMask;

    public static GenerateTrees instance { get; private set; }
    public Mesh mesh;
    public Dictionary<Vector3, bool> TreePos;
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }

    void Start()
    {
        ClearLists();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*public void GenTrees() *** DEPRECATED ***
    {
        uvs = new List<Vector2>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        for (int i = 0; i < numTrees; i++)
        {
            RaycastHit hit;
            int randx = (int)Random.Range(0f, vr.xSize);
            int randz = (int)Random.Range(0f, vr.zSize);
            Vector3 start = new Vector3(randx, 1000f, randz);
            if (Physics.Raycast(start, Vector3.up * -1f, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawLine(start, start - new Vector3(start.x, start.y - 1000f, start.z), Color.green, 15f);
                MakeTree(new Vector3((int)hit.point.x -2.5f, (int)hit.point.y, (int)hit.point.z -2.5f));
            }
        }
        UpdateMesh();
    }*/

    public void ClearLists()
    {
        TreePos = new Dictionary<Vector3, bool>();
        uvs = new List<Vector2>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    public void MakeTree(Vector3 offset)
    {

        bool isLeaf;
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (vd.data[x, y, z] == 1)
                    {
                        if(!TreePos.ContainsKey(offset + new Vector3(x, y, z)))
                        {
                            if (y >= 3)
                            {
                                isLeaf = true;
                            }
                            else
                            {
                                isLeaf = false;
                            }
                            TreePos.Add(offset + new Vector3(x, y, z), isLeaf);
                            MakeCube(offset + new Vector3(x,y,z), isLeaf);
                        }
                    }
                }
            }
        }
    }
    public void MakeCube(Vector3 cubePos, bool isLeaf)
    {
        if (!TreePos.ContainsKey(cubePos))
        {
            TreePos.Add(cubePos, isLeaf);
        }
        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubePos);
            AssignUVs(i, isLeaf);
        }
    }


    void AssignUVs(int dir, bool isLeaf)
    {
        if (isLeaf)
        {
            switch (dir)
            {
                case 0://North
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0, 0.5f));
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0.5f, 0));
                    break;
                case 1://Right (Rotated 1 -> 4)
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0, 0.5f));
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0.5f, 0));
                    break;
                case 2://South
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0, 0.5f));
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0.5f, 0));
                    break;
                case 3://Left (Rotated 4 -> 1)
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0, 0.5f));
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0.5f, 0));
                    break;
                case 4://Top
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0, 0.5f));
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0.5f, 0));
                    break;
                case 5://Bottom
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0, 0.5f));
                    uvs.Add(new Vector2(0, 0));
                    uvs.Add(new Vector2(0.5f, 0));
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }
        else
        {
            switch (dir)
            {
                case 0://North
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 1f));
                    uvs.Add(new Vector2(0.5f, 1f));
                    break;
                case 1://Right (Rotated 1 -> 4)
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 1f));
                    uvs.Add(new Vector2(0.5f, 1f));
                    break;
                case 2://South
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 1f));
                    uvs.Add(new Vector2(0.5f, 1f));
                    break;
                case 3://Left (Rotated 4 -> 1)
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 0.5f));
                    uvs.Add(new Vector2(0.0f, 1f));
                    uvs.Add(new Vector2(0.5f, 1f));
                    break;
                case 4://Top
                    uvs.Add(new Vector2(1f, 0.5f));
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0.5f, 0));
                    uvs.Add(new Vector2(1f, 0));
                    break;
                case 5://Bottom
                    uvs.Add(new Vector2(1f, 0.5f));
                    uvs.Add(new Vector2(0.5f, 0.5f));
                    uvs.Add(new Vector2(0.5f, 0));
                    uvs.Add(new Vector2(1f, 0));
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }

    }

    void MakeFace(int dir, Vector3 facePos)
    {
        //Debug.Log("Making face" + dir + "at face pos" + facePos);
        vertices.AddRange(CubeMeshData.faceVertices(dir, facePos));
        int vcount = vertices.Count;

        triangles.Add(vcount - 4);
        triangles.Add(vcount - 4 + 1);
        triangles.Add(vcount - 4 + 2);
        triangles.Add(vcount - 4);
        triangles.Add(vcount - 4 + 2);
        triangles.Add(vcount - 4 + 3);
    }

    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;
    }
    public void DestroyCube(int tIndex, Vector3 cubePos)
    {
        //Debug.Log(tIndex);
        int lowestNearestTriangle = tIndex - (tIndex % 12); //Get lowest triangle multiple of 36 E.G 82 - 82 % 36 = 72
        int lowestNearestVertex = (lowestNearestTriangle / 12) * 24; //Get equivalent starting vertex for that triangle.
        //Debug.Log("Lowest nearest triangle index: " + lowestNearestTriangle);
        //Debug.Log("Lowest nearest vertex index: " + lowestNearestVertex);
        for (int i = lowestNearestTriangle * 3; i < lowestNearestTriangle * 3 + 36; i++)//Remove next 36 triangle formations.
        {
            triangles[i] = 0;
        }
        for (int i = lowestNearestVertex; i < lowestNearestVertex + 24; i++)//Remove next 24 vertices.
        {
            vertices[i] = Vector3.zero;
            uvs[i] = Vector2.zero;
        }
        TreePos.Remove(cubePos);
        UpdateMesh();
    }
}
