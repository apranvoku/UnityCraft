using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class VoxelRender : MonoBehaviour
{
    public int xSize;
    public int zSize;

    public GenerateTrees GT;
    public NavMeshSurface surface;
    public static VoxelRender instance { get; private set; }
    public Mesh mesh;
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;

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


    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
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
    // Start is called before the first frame update
    void Start()
    {
        GT = GameObject.Find("GenerateTrees").GetComponent<GenerateTrees>();
        //xSize = 40;
        //zSize = 40;
    }

#if UNITY_EDITOR
    public bool initializeMesh = false;

    //You can have multiple booleans here
    private void OnValidate()
    {
        if (initializeMesh)
        {
            // Your function here
            //StartCoroutine(Generation());
            GenerateVoxelMesh();
            UpdateMesh();
            GT.UpdateMesh();


            //When its done set this bool to false
            //This is useful if you want to do some stuff only when clicking this "button"
            initializeMesh = false;
        }
    }
#endif
    void GenerateVoxelMesh()
    {
        XRandOffset1 = Random.Range(-10000, 10000);
        ZRandOffset1 = Random.Range(-10000, 10000);
        XRandOffset2 = Random.Range(-10000, 10000);
        ZRandOffset2 = Random.Range(-10000, 10000);
        XRandOffset3 = Random.Range(-10000, 10000);
        ZRandOffset3 = Random.Range(-10000, 10000);

        uvs = new List<Vector2>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        GT.ClearLists();
        //StartCoroutine(Generation());
            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    y = ((lowFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset1) / 100f, (z + ZRandOffset1) / 200f))) - 1)) // Low Frequency
                       + (midFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset3) / 33f, (z + ZRandOffset3) / 30f))) - 1))  // Med Frequency
                       + (highFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset2) / 8f, (z + ZRandOffset2) / 4f))) - 1))); // High Frequency
                    if (y > -lowFreqAmp)
                    {
                        MakeCube(new Vector3(x, (int)y + 100, z));
                        if(Random.Range(1, 50) > 48)
                        {
                            GT.MakeTree(new Vector3(x-2, (int)y + 101, z-2));
                        }
                        MakeCube(new Vector3(x, (int)y + 99, z));
                        //Debug.Log("making cube at : " + x + " : "+ (int)y + " : " + z);
                        //Debug.Log("Perlin result: " + y);
                        //GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(x, (int)y - 102, z, "Dirt");
                    }


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


    public void MakeCube(Vector3 cubePos)
    {
        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubePos);
            AssignUVs(i);
        }
    }

    public void DestroyCube(int tIndex)
    {
        //Debug.Log(tIndex);
        int lowestNearestTriangle = tIndex - (tIndex % 12); //Get lowest triangle multiple of 36 E.G 82 - 82 % 36 = 72
        int lowestNearestVertex = (lowestNearestTriangle / 12) * 24; //Get equivalent starting vertex for that triangle.
        //Debug.Log("Lowest nearest triangle index: " + lowestNearestTriangle);
        //Debug.Log("Lowest nearest vertex index: " + lowestNearestVertex);
        for (int i = lowestNearestTriangle*3; i < lowestNearestTriangle*3 + 36; i++)//Remove next 36 triangle formations.
        {
            triangles[i] = 0;
        }
        for (int i = lowestNearestVertex; i < lowestNearestVertex + 24; i++)//Remove next 24 vertices.
        {
            vertices[i] = Vector3.zero;
            uvs[i] = Vector2.zero;
        }

        UpdateMesh();
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
    // Update is called once per frame
    private void Update()
    {
        
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

        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface.BuildNavMesh();
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
        for (int x = 0; x < 30; x++)
        {
            for (int z = 0; z < 30; z++)
            {
                    y = ((lowFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset1) / 200f, (z + ZRandOffset1) / 200f))) - 1)) // Low Frequency
                       + (midFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset3) / 30f, (z + ZRandOffset3) / 30f))) - 1))  // Med Frequency
                       + (highFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset2) / 4f, (z + ZRandOffset2) / 4f))) - 1))); // High Frequency
                    StartCoroutine(DelayedMakeCube(new Vector3(x, (int)y - 100, z)));
                    //MakeCube(new Vector3(x,(int)y - 100, z));
                    //Debug.Log("Perlin result: " + y);
                    //GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(x, (int)y - 102, z, "Dirt");
                    yield return new WaitForSeconds(0.05f);
            }
        }
    }
}