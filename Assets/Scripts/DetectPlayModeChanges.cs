using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public class DetectPlayModeChanges : MonoBehaviour
{

    static string fileName = "/save.txt";
    static string buildString;
    static string[] readString;

    static DetectPlayModeChanges()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    public static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        string dir = Application.persistentDataPath + fileName;
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                // Do whatever before entering play mode

                // Do whatever after entering play mode
                break;
            case PlayModeStateChange.EnteredPlayMode:
                readString = File.ReadAllLines(dir);
                foreach(string shortString in readString)
                {
                    //Debug.Log(shortString);
                    string[] splitString = shortString.Split(":");//index 0 - 2 is position, 3 is name.
                    GameObject.Find("Head").transform.GetComponent<PlaceBlock>().PlaceNewBlock(float.Parse(splitString[0]), float.Parse(splitString[1]), float.Parse(splitString[2]), splitString[3]);
                    //Debug.Log("Here");
                }
                break;
            case PlayModeStateChange.ExitingPlayMode:
                Debug.Log("Saving saveables to " + Application.persistentDataPath);
                Transform Saveables = GameObject.Find("SaveableBlocks").transform;

                foreach(Transform child in Saveables)
                {
                    buildString += new string(child.transform.position.x + ":" + child.transform.position.y + ":" + child.transform.position.z + ":" + child.transform.name.Split("(")[0]);
                    buildString += "\n";
                    //Debug.Log(buildString);
                }
                //File.WriteAllText(dir, buildString); //UNCOMMENT THIS LINE TO SAVE

                // Do whatever before returning to edit mode
                break;
            case PlayModeStateChange.EnteredEditMode:
                // Do whatever after returning to edit mode
                break;
        }
    }
}