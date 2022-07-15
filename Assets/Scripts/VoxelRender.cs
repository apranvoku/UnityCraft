using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class VoxelRender : MonoBehaviour
{
    public Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;

    public float lowFreqAmp = 30f;
    public float midFreqAmp = 6f;
    public float highFreqAmp = 1f;

    float XRandOffset1;
    float ZRandOffset1;
    float XRandOffset2;
    float ZRandOffset2;
    float XRandOffset3;
    float ZRandOffset3;

    float y;

    public bool locked;
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }
    // Start is called before the first frame update
    void Start()
    {
        locked = false;
        XRandOffset1 = Random.Range(-10000, 10000);
        ZRandOffset1 = Random.Range(-10000, 10000);
        XRandOffset2 = Random.Range(-10000, 10000);
        ZRandOffset2 = Random.Range(-10000, 10000);
        XRandOffset3 = Random.Range(-10000, 10000);
        ZRandOffset3 = Random.Range(-10000, 10000);
        GenerateVoxelMesh(new VoxelData());
        UpdateMesh();

    }

    void GenerateVoxelMesh(VoxelData data)
    {
        uvs = new List<Vector2>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        //StartCoroutine(Generation());
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                        y = ((lowFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset1) / 200f, (z + ZRandOffset1) / 200f))) - 1)) // Low Frequency
                           + (midFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset3) / 30f, (z + ZRandOffset3) / 30f))) - 1))  // Med Frequency
                           + (highFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset2) / 4f, (z + ZRandOffset2) / 4f))) - 1))); // High Frequency
                        MakeCube(new Vector3(x,(int)y + 100, z));
                        //Debug.Log("Perlin result: " + y);
                        //GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(x, (int)y - 102, z, "Dirt");

                    
                }
            }
    }
    /*
    for (int z = 0; z < data.Depth; z++)
    {
        for(int x = 0; x < data.Width; x++)
        {
            if(data.GetCell(x, z) == 0)
            {
                continue;
            }
            MakeCube(new Vector3(x, 0, z));
        }
    }*/


    void MakeCube(Vector3 cubePos)
    {
        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubePos);
            AssignUVs(i);
        }
    }

    void AssignUVs(int dir)
    {
        switch(dir)
        {
            case 0://North
                uvs.Add(new Vector2(1f, 0.5f));
                uvs.Add(new Vector2(0.5f, 0.5f));
                uvs.Add(new Vector2(0.5f, 0));
                uvs.Add(new Vector2(1f, 0));
                break;
            case 1://Right (Rotated 1 -> 4)
                uvs.Add(new Vector2(1f, 0.5f));
                uvs.Add(new Vector2(0.5f, 0.5f));
                uvs.Add(new Vector2(0.5f, 0));
                uvs.Add(new Vector2(1f, 0));
                break;
            case 2://South
                uvs.Add(new Vector2(0.5f,0.5f));
                uvs.Add(new Vector2(1f, 0.5f));
                uvs.Add(new Vector2(1f, 0));
                uvs.Add(new Vector2(0.5f, 0));
                break;
            case 3://Left (Rotated 4 -> 1)
                uvs.Add(new Vector2(0.5f, 0.5f));
                uvs.Add(new Vector2(1f, 0.5f));
                uvs.Add(new Vector2(1f, 0));
                uvs.Add(new Vector2(0.5f, 0));
                break;
            case 4://Top
                uvs.Add(new Vector2(0.5f, 0.5f));
                uvs.Add(new Vector2(0.0f, 0.5f));
                uvs.Add(new Vector2(0.0f, 1f));
                uvs.Add(new Vector2(0.5f, 1f));
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
    private void Update()
    {
        
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;
    }

    public IEnumerator DelayedMakeCube(Vector3 pos)
    {
        Debug.Log("building at " + pos.ToString());
        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, pos);
            yield return new WaitForSeconds(0.1f);
            AssignUVs(i);
            //yield return new WaitForSeconds(0.1f);
        }
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
    public IEnumerator Generation()
    {
        for (int x = 0; x < 20; x++)
        {
            for (int z = 0; z < 20; z++)
            {
                    y = ((lowFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset1) / 200f, (z + ZRandOffset1) / 200f))) - 1)) // Low Frequency
                       + (midFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset3) / 30f, (z + ZRandOffset3) / 30f))) - 1))  // Med Frequency
                       + (highFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset2) / 4f, (z + ZRandOffset2) / 4f))) - 1))); // High Frequency
                    StartCoroutine(DelayedMakeCube(new Vector3(x, (int)y - 100, z)));
                    //MakeCube(new Vector3(x,(int)y - 100, z));
                    //Debug.Log("Perlin result: " + y);
                    //GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(x, (int)y - 102, z, "Dirt");
                    yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
