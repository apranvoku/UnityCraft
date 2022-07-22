using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public class SaveLoadVoxelMesh : MonoBehaviour
{
    static string fileName = "/meshSave.txt";
    static string buildString;
    static string[] readString;

    static SaveLoadVoxelMesh()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    public static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        string dir = Application.persistentDataPath + fileName;
        Debug.Log(Application.persistentDataPath);
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                // Do whatever before entering play mode

                // Do whatever after entering play mode
                break;
            case PlayModeStateChange.EnteredPlayMode://Make blocks using save data.
                Debug.Log("Restoring voxel mesh from file...");
                readString = File.ReadAllLines(dir);
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
                        float.Parse(sArray[0])-1f,
                        float.Parse(sArray[1])-1f,
                        float.Parse(sArray[2])-1f);

                    VoxelRender.instance.MakeCube(result);
                }
                VoxelRender.instance.UpdateMesh();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                Debug.Log("Saving saveables to " + Application.persistentDataPath);

                for (int v = 0; v < VoxelRender.instance.vertices.Count; v += 24)
                {
                    buildString += new string(VoxelRender.instance.vertices[v].ToString());
                    buildString += "\n";
                }
                File.WriteAllText(dir, buildString); //UNCOMMENT THIS LINE TO SAVE

                // Do whatever before returning to edit mode
                break;
            case PlayModeStateChange.EnteredEditMode:
                // Do whatever after returning to edit mode
                break;
        }
    }
}
