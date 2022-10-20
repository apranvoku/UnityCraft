using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    public GameObject voxelMesh;
    public string SaveID;

    void Start()
    {
        Debug.Log("Save Directory is: " + Application.persistentDataPath + "/saves/");
    }
    public void SetSaveID(string ID)
    {
        SaveID = ID;
    }
    public void LoadSave()
    {
        string meshdir = Application.persistentDataPath + "/saves/" + SaveID + "/mesh";
        string treedir = Application.persistentDataPath + "/saves/" + SaveID + "/trees";
        string blockdir = Application.persistentDataPath + "/saves/" + SaveID + "/gameobjects";

        string[] readString = File.ReadAllLines(blockdir + "/go.txt");
        foreach (string shortString in readString)
        {
            string[] splitString = shortString.Split(":");//index 0 - 2 is position, 3 is name.
            GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(float.Parse(splitString[0]), float.Parse(splitString[1]), float.Parse(splitString[2]), splitString[3]);
            //Debug.Log("Here");
        }

        int chunkID = 0;
        foreach (string chunk in System.IO.Directory.GetFiles(meshdir))
        {
            VoxelRender render = Instantiate(voxelMesh, GameObject.Find("VoxelMeshParent").transform).GetComponent<VoxelRender>();
            string chunkPath = meshdir + "/chunk" + chunkID.ToString();
            string[] meshStringList = File.ReadAllLines(chunkPath);
            foreach (string shortString in meshStringList)
            {
                string sVector = "";
                // Remove the parentheses
                if (shortString.StartsWith("(") && shortString.EndsWith(")"))
                {
                    sVector = shortString.Substring(1, shortString.Length - 2);
                }

                // split the items
                string[] sArray = sVector.Split(',');
                //Debug.Log(sArray.ToString());

                // store as a Vector3
                Vector3 result = new Vector3(
                    float.Parse(sArray[0]) - 1f,
                    float.Parse(sArray[1]) - 1f,
                    float.Parse(sArray[2]) - 1f);


                render.MakeCube(result);
                //VoxelRender.instance.MakeCube(result);
            }
            render.UpdateMesh();
            //VoxelRender.instance.UpdateMesh();
            chunkID++;
        }

        string treePath = treedir + "/tree";
        string[] treeStringList = File.ReadAllLines(treePath);
        foreach (string keyVal in treeStringList)
        {
            string[] pairList = keyVal.Split(":");
            string positionString = pairList[0];
            string isLeafString = pairList[1];
            string sVector = "";
            // Remove the parentheses
            if (positionString.StartsWith("(") && positionString.EndsWith(")"))
            {
                sVector = positionString.Substring(1, positionString.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');
            //Debug.Log(sArray.ToString());

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            bool isLeaf = Boolean.Parse(isLeafString);
            GenerateTrees.instance.MakeCube(result, isLeaf);
            
            //VoxelRender.instance.MakeCube(result);
        }
        GenerateTrees.instance.UpdateMesh();
    }

    public void SaveWorld()
    {
        string meshdir = Application.persistentDataPath + "/saves/" + SaveID + "/mesh";
        string treedir = Application.persistentDataPath + "/saves/" + SaveID + "/trees";
        string blockdir = Application.persistentDataPath + "/saves/" + SaveID + "/gameobjects";

        string mesh_string = string.Empty;
        string tree_string = string.Empty;
        string go_string = string.Empty;

        Debug.Log("Saving saveables to " + Application.persistentDataPath);
        Transform gameObjects = GameObject.Find("SaveableBlocks").transform;
        Transform chunks = GameObject.Find("VoxelMeshParent").transform;

        
        foreach (Transform child in gameObjects)
        {
            go_string += new string(child.transform.position.x + ":" + child.transform.position.y + ":" + child.transform.position.z + ":" + child.transform.name.Split("(")[0]);
            go_string += "\n";
            //Debug.Log(buildString);
        }
        File.WriteAllText(blockdir + "/go.txt", go_string); //UNCOMMENT THIS LINE TO SAVE

        int chunkID = 0;
        foreach (Transform child in chunks)
        {
            mesh_string = string.Empty;
            VoxelRender chunk = child.GetComponent<VoxelRender>();
            for (int v = 0; v < chunk.vertices.Count; v += 24)
            {
                mesh_string += new string(chunk.vertices[v].ToString());
                mesh_string += "\n";
            }
            File.WriteAllText(meshdir + "/chunk" + chunkID.ToString(), mesh_string);//UNCOMMENT THIS LINE TO SAVE
            chunkID++;
        }

        foreach (var pair in GenerateTrees.instance.TreePos)
        {
            tree_string += pair.Key.ToString() + ":" + pair.Value.ToString();
            tree_string += "\n";
        }
        File.WriteAllText(treedir + "/tree", tree_string);//UNCOMMENT THIS LINE TO SAVE
    }
}