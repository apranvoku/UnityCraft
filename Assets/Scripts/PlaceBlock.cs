using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

public class PlaceBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Plank;
    public GameObject Oak;
    public GameObject Glowstone;
    public GameObject Glass;
    public GameObject Dirt;

    public List<GameObject> Blocks;
    public GameObject CanvasBlocks;
    public GameObject Border;
    public GameObject SaveableBlocks;
    public int index;
    int layerMask;
    void Start()
    {
        Blocks = new List<GameObject>();

        Blocks.Add(Plank);
        Blocks.Add(Oak);
        Blocks.Add(Glowstone);
        Blocks.Add(Glass);
        Blocks.Add(Dirt);

        index = Blocks.Count - 1;

        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << 2;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))//Does the raycast hit anything
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 0.1f);
                //Vector3 nearestPoint = GetApproximateCornerVertex(hit.point, hit.transform.GetComponent<MeshFilter>().mesh, hit.normal);
                Vector3 nearestPoint = GetApproximateCornerVertexTriangle(hit.triangleIndex, hit.normal, hit.transform.gameObject.GetComponent<VoxelRender>());
                Debug.DrawLine(nearestPoint, nearestPoint + hit.normal, Color.red, 5f);
                /*VoxelRender.instance.MakeCube(nearestPoint);//Add cube to mesh
                VoxelRender.instance.UpdateMesh();*/ //
                if(hit.transform.tag == "Voxel")
                {
                    Instantiate(Blocks[index], nearestPoint, Blocks[index].transform.rotation, SaveableBlocks.transform);
                }
                else
                {
                    Instantiate(Blocks[index], hit.transform.position + hit.normal, Blocks[index].transform.rotation, SaveableBlocks.transform);
                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            index++;
            index = (index % Blocks.Count);
            Border.transform.position = CanvasBlocks.transform.GetChild(index).position;
            //Debug.Log(index);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            index--;
            if (index < 0)
            {
                index += Blocks.Count;
            }
            Border.transform.position = CanvasBlocks.transform.GetChild(index).position;
            //Debug.Log(index);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))//Does the raycast hit anything
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue, 0.5f);
                if (hit.transform.gameObject.name != "Player")
                {
                    if (hit.transform.tag == "Voxel")
                    {
                        if(hit.transform.name.Contains("VoxelMesh"))
                        {
                            hit.transform.GetComponent<VoxelRender>().DestroyCube(hit.triangleIndex);
                        }
                        if(hit.transform.name == "GenerateTrees")
                        {
                            GenerateTrees.instance.DestroyCube(hit.triangleIndex);
                        }
                    }
                    else
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
        }
    }
    public Vector3 GetApproximateCornerVertexTriangle(int tindex, Vector3 normal, VoxelRender vr)
    {
        List<Vector3> tpoints = new List<Vector3>();
        if (tindex % 2 == 0)//Even
        {
            tpoints.Add(vr.vertices[(tindex / 2) * 4]);
            tpoints.Add(vr.vertices[(tindex / 2) * 4 + 1]);
            tpoints.Add(vr.vertices[(tindex / 2) * 4 + 2]);
        }
        else//Odd
        {
            tpoints.Add(vr.vertices[(tindex / 2) * 4]);
            tpoints.Add(vr.vertices[(tindex / 2) * 4 + 2]);
            tpoints.Add(vr.vertices[(tindex / 2) * 4 + 3]);
        }
        float x = 0; float y = 0; float z = 0;
        for (int j = 0; j < tpoints.Count; j++)
        {
            x += tpoints[j].x;
            y += tpoints[j].y;
            z += tpoints[j].z;
        }
        /*Debug.Log(tindex);
        foreach (Vector3 p in tpoints)
        {
            Debug.Log(p);
        }*/
        if (normal.x < 0 || normal.y < 0 || normal.z < 0)
        {
            return new Vector3(Mathf.Floor(x / 3f), Mathf.Floor(y / 3), Mathf.Floor(z / 3)) + normal;
        }
        else
        {
            return new Vector3(Mathf.Floor(x / 3f), Mathf.Floor(y / 3), Mathf.Floor(z / 3));
        }
    }
    public Vector3 GetApproximateCornerVertex(Vector3 point, Mesh mesh, Vector3 normal)
    {
        SortedDictionary<float, Vector3> pointDist = new SortedDictionary<float, Vector3>();
        // scan all vertices to find nearest
        int entries = 0;
        foreach (Vector3 vertex in mesh.vertices)
        {
            Vector3 diff = point - vertex;
            float distance = diff.magnitude;
            try
            {
                pointDist.Add(distance, vertex);
                entries++;
                if (entries > 3)
                {
                    pointDist.Remove(pointDist.Keys.Max());
                }
            }
            catch (System.ArgumentException e)
            {
                continue;
            }
        }
        // convert nearest vertex back to world space
        int i = 0;
        Vector3[] closestPoints = new Vector3[3];
        foreach (KeyValuePair<float, Vector3> pair in pointDist)
        {
            closestPoints[i] = pair.Value;
            Debug.Log(pair.Key + " : " + pair.Value);
            if(pair.Key >= 1f)
            {
                throw new Exception();
            }
            i++;
        }
        float x = 0; float y = 0; float z = 0;
        for (int j = 0; j < closestPoints.Length; j++)
        {
            x += closestPoints[j].x;
            y += closestPoints[j].y;
            z += closestPoints[j].z;
        }
        if (normal.x < 0 || normal.y < 0 || normal.z < 0)
        {
            return new Vector3(Mathf.Floor(x / 3f), Mathf.Floor(y / 3), Mathf.Floor(z / 3)) + normal;
        }
        else
        {
            return new Vector3(Mathf.Floor(x / 3f), Mathf.Floor(y / 3), Mathf.Floor(z / 3));
        }
    }




public void PlaceNewBlock(float x, float y, float z, string name)
    {
        //Debug.Log(x + " " + y + " " + z + " " + name);
        if (name == "Block1")
        {
            Instantiate(Plank, new Vector3(x, y, z), Plank.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Block2")
        {
            Instantiate(Oak, new Vector3(x, y, z), Oak.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Block3")
        {
            Instantiate(Glowstone, new Vector3(x, y, z), Glowstone.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Block4")
        {
            Instantiate(Glass, new Vector3(x, y, z), Glass.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Block5")
        {
            Instantiate(Dirt, new Vector3(x, y, z), Dirt.transform.rotation, SaveableBlocks.transform);
        }
    }
}
