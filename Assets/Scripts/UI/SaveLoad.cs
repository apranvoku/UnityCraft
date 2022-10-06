using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    static string buildString;
    static string[] readString;
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
        string dir1 = Application.persistentDataPath + "/saves/" + SaveID + "_block";
        readString = File.ReadAllLines(dir1);
        foreach (string shortString in readString)
        {
            //Debug.Log(shortString);
            string[] splitString = shortString.Split(":");//index 0 - 2 is position, 3 is name.
            GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(float.Parse(splitString[0]), float.Parse(splitString[1]), float.Parse(splitString[2]), splitString[3]);
            //Debug.Log("Here");
        }
        string dir2 = Application.persistentDataPath + "/saves/" + SaveID + "_mesh";
        readString = File.ReadAllLines(dir2);
        foreach (string shortString in readString)
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

            VoxelRender.instance.MakeCube(result);
        }
        VoxelRender.instance.UpdateMesh();
    }

    public void SaveWorld()
    {
        string dir1 = Application.persistentDataPath + "/saves/" + SaveID + "_block";
        Debug.Log("Saving saveables to " + Application.persistentDataPath);
        Transform Saveables = GameObject.Find("SaveableBlocks").transform;

        foreach (Transform child in Saveables)
        {
            buildString += new string(child.transform.position.x + ":" + child.transform.position.y + ":" + child.transform.position.z + ":" + child.transform.name.Split("(")[0]);
            buildString += "\n";
            //Debug.Log(buildString);
        }
        File.WriteAllText(dir1, buildString); //UNCOMMENT THIS LINE TO SAVE

        string dir2 = Application.persistentDataPath + "/saves/" + SaveID + "_mesh";
        for (int v = 0; v < VoxelRender.instance.vertices.Count; v += 24)
        {
            buildString += new string(VoxelRender.instance.vertices[v].ToString());
            buildString += "\n";
        }
        File.WriteAllText(dir2, buildString); //UNCOMMENT THIS LINE TO SAVE
    }
}