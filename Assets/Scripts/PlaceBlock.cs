using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlaceBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Plank;
    public GameObject Oak;
    public GameObject Grass;
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
        Blocks.Add(Grass);
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
                Vector3 nearestPoint = GetNearestVertex(hit.point, hit.transform.GetComponent<MeshFilter>().mesh);
                //Debug.Log(nearestPoint);
                Instantiate(Blocks[index], nearestPoint + hit.normal, Blocks[index].transform.rotation, SaveableBlocks.transform);
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
                if(hit.transform.gameObject.name != "Player")
                {
                    Destroy(hit.transform.gameObject);
                }
               
            }
        }
    }
    public Vector3 GetNearestVertex(Vector3 point, Mesh mesh)
    {
        SortedDictionary<float, Vector3> pointDist = new SortedDictionary<float, Vector3>();
        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;
        // scan all vertices to find nearest
        foreach (Vector3 vertex in mesh.vertices)
        {
            Vector3 diff = point - vertex;
            float distSqr = diff.sqrMagnitude;
            try
            {
                pointDist.Add(distSqr, point);
            }
            catch(System.ArgumentException e)
            {
                continue;
            }
            /*if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }*/
        }
        // convert nearest vertex back to world space
        int i = 0;
        Vector3[] closestPoints = new Vector3[4];
        foreach (KeyValuePair<float, Vector3> pair in pointDist)
        {
            closestPoints[i] = pair.Value;
            Debug.Log(pair.Key);
            Debug.Log(pair.Value);
            i++;
            if(i == 4)
            {
                break;
            }
        }
        float x = 0; float y = 0; float z = 0;
        for(int j = 0; j < closestPoints.Length; j++)
        {
            x += closestPoints[j].x;
            y += closestPoints[j].y;
            z += closestPoints[j].z;
        }
        return new Vector3(x/4f, y/4, z/4);
    }




public void PlaceNewBlock(float x, float y, float z, string name)
    {
        //Debug.Log(x + " " + y + " " + z + " " + name);
        if (name == "Plank")
        {
            Instantiate(Plank, new Vector3(x, y, z), Plank.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Oak")
        {
            Instantiate(Oak, new Vector3(x, y, z), Oak.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Grass")
        {
            Instantiate(Grass, new Vector3(x, y, z), Grass.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Glass")
        {
            Instantiate(Glass, new Vector3(x, y, z), Grass.transform.rotation, SaveableBlocks.transform);
        }
        if (name == "Dirt")
        {
            Instantiate(Dirt, new Vector3(x, y, z), Grass.transform.rotation, SaveableBlocks.transform);
        }
    }
}
