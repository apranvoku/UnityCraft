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

    public List<GameObject> Blocks;
    public GameObject CanvasBlocks;
    public GameObject Border;
    public GameObject SaveableBlocks;
    private int index;
    int layerMask;
    void Start()
    {
        Blocks = new List<GameObject>();

        Blocks.Add(Plank);
        Blocks.Add(Oak);
        Blocks.Add(Grass);
        Blocks.Add(Glass);

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
                Instantiate(Blocks[index], hit.transform.position + hit.normal, Blocks[index].transform.rotation, SaveableBlocks.transform);
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
    }
}
