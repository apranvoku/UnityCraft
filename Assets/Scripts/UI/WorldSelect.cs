using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class WorldSelect : MonoBehaviour
{
    TMP_Dropdown dd;
    // Start is called before the first frame update
    void Start()
    {
        dd = GetComponent<TMP_Dropdown>();
        UpdateDropDown();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateDropDown()
    {
        int ID = 0;
        string path = Application.persistentDataPath + "/saves";
        foreach (string directory in System.IO.Directory.GetDirectories(path))
        {
            dd.AddOptions(new List<string> { "World: " + ID });
            ID++;
        }
    }

    public int GetSelection()
    {
        return dd.value;
    }
}
