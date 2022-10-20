using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class VoxelRender : MonoBehaviour
{
    public NavMeshSurface surface;
    public MeshCollider mc;
    public MeshFilter mf;
    public Mesh mesh;

    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;

    public float lowFreqAmp = 30f;
    public float midFreqAmp = 6f;
    public float highFreqAmp = 1f;

    float y;


    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        mesh = GetComponent<MeshFilter>().mesh;
        mc = GetComponent<MeshCollider>();
        mf = GetComponent<MeshFilter>();
        //mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void GenerateVoxelMesh(float customLowFreqAmp, float customMedFreqAmp, float customHighFreqAmp, int start_x, int start_z,
        float XRandOffset1, float ZRandOffset1, float XRandOffset2, float ZRandOffset2, float XRandOffset3, float ZRandOffset3, int TreeDensity)
    {
        uvs = new List<Vector2>();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        //StartCoroutine(Generation());
        for (int x = start_x; x < start_x + 32; x++)
        {
            for (int z = start_z; z < start_z + 32; z++)
            {
                y = ((customLowFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset1) / 100f, (z + ZRandOffset1) / 200f))) - 1)) // Low Frequency
                   + (customMedFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset2) / 33f, (z + ZRandOffset2) / 30f))) - 1))  // Med Frequency
                   + (customHighFreqAmp * ((2 * (Mathf.PerlinNoise((x + XRandOffset3) / 8f, (z + ZRandOffset3) / 4f))) - 1))); // High Frequency
                if (y > -lowFreqAmp)
                {
                    MakeCube(new Vector3(x, (int)y + 100, z));
                    if (Random.Range(1, 1000) < TreeDensity)
                    {
                        GenerateTrees.instance.MakeTree(new Vector3(x - 2, (int)y + 101, z - 2));
                    }
                    //MakeCube(new Vector3(x, (int)y + 99, z));
                    //Debug.Log("making cube at : " + x + " : "+ (int)y + " : " + z);
                    //Debug.Log("Perlin result: " + y);
                    //GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(x, (int)y - 102, z, "Dirt");
                }
            }
        }
    }

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
        int lowestNearestTriangle = tIndex - (tIndex % 12); //Get lowest triangle multiple of 36 E.G 72 - 72 % 36 = 72
        int lowestNearestVertex = (lowestNearestTriangle / 12) * 24; //Get equivalent starting vertex for that triangle.
        //Debug.Log("Lowest nearest triangle index: " + lowestNearestTriangle);
        //Debug.Log("Lowest nearest vertex index: " + lowestNearestVertex);
        for (int i = lowestNearestTriangle * 3; i < lowestNearestTriangle * 3 + 36; i++)//Remove next 36 triangle formations.
        {
            triangles[i] = 0;
        }
        Vector3 zero = Vector3.zero;
        Vector2 zero2 = Vector2.zero;
        for (int i = lowestNearestVertex; i < lowestNearestVertex + 24; i++)//Remove next 24 vertices.
        {
            vertices[i] = zero;
            uvs[i] = zero2;
        }
        EasyUpdateMesh();
    }

    void AssignUVs(int dir)
    {
        switch (dir)
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
                uvs.Add(new Vector2(0.5f, 0.5f));
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
    public void EasyUpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mc.sharedMesh = mf.mesh;
    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        mc.sharedMesh = null;
        mc.sharedMesh = mf.mesh;

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
    /*public IEnumerator Generation() ***DEPRECATED***
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
    }*/
}