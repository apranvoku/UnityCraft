using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject voxelMesh;
    public int x_size;
    public int z_size;
    Button butt;
    private Quaternion light_rot;
    public PlayerController playerController;
    public GameObject directionalLight;
    public GameObject CanvasMenu;
    public GameObject CanvasPersistent;
    public WorldSelect worldSelect;
    public SaveLoad SL;
    public TextMeshProUGUI saveInfo;
    public TMP_InputField chunkWidth;
    public TMP_InputField chunkLength;
    public TMP_InputField lowAmp;
    public TMP_InputField medAmp;
    public TMP_InputField highAmp;
    public TMP_InputField treeDensity;

    public Vector3 Spawn;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Steve").GetComponent<PlayerController>();
        chunkWidth = GameObject.Find("TerrainWidth").GetComponent<TMP_InputField>();
        chunkLength = GameObject.Find("TerrainLength").GetComponent<TMP_InputField>();
        lowAmp = GameObject.Find("LowAmp").GetComponent<TMP_InputField>();
        medAmp = GameObject.Find("MedAmp").GetComponent<TMP_InputField>();
        highAmp = GameObject.Find("HighAmp").GetComponent<TMP_InputField>();
        treeDensity = GameObject.Find("TreeDensity").GetComponent<TMP_InputField>();
        saveInfo = GameObject.Find("SaveInfo").GetComponent<TextMeshProUGUI>();
        directionalLight = GameObject.Find("Directional Light");
        light_rot = directionalLight.transform.localRotation;
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
        CanvasMenu = GameObject.Find("CanvasMenu");
        CanvasPersistent = GameObject.Find("CanvasPersistent");
        worldSelect = GameObject.Find("WorldSelect").GetComponent<WorldSelect>();
        SL = GetComponent<SaveLoad>();
        butt = GetComponent<Button>();
        butt.onClick.AddListener(StartGameFun);
        InitializeSaveDirectory();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SL.SaveWorld();
            saveInfo.text = "Saved World!";
            StartCoroutine(ResetSaveInfo());
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject.Find("Steve").transform.position = Spawn;
            playerController.rb.velocity = Vector3.zero;
        }
    }

    public IEnumerator ResetSaveInfo()
    {
        yield return new WaitForSeconds(3f);
        saveInfo.text = "Press e to save...";
    }

    public void SetSteveStart()
    {
        RaycastHit hit;
        int x_mid = 16;
        int z_mid = 16;
        Vector3 start = new Vector3(x_mid, 1000f, z_mid);
        if (Physics.Raycast(start, Vector3.up * -1f, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(start, start - new Vector3(start.x, start.y - 1000f, start.z), Color.green, 15f);
            Spawn = new Vector3((int)hit.point.x, (int)hit.point.y + 10, (int)hit.point.z);
            GameObject.Find("Steve").transform.position = Spawn;
        }

    }

    private void InitializeSaveDirectory()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            //if save folder doesn't exist, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
    }
    void StartGameFun()
    {
        Debug.Log("Generating...");
        playerController.Gravity();
        directionalLight.transform.localRotation = light_rot;
        directionalLight.GetComponent<DayNightCycle>().active = true;
        Cursor.lockState = CursorLockMode.Locked;
        if (worldSelect.GetSelection() == 0)
        {
            if (chunkWidth.text == "")
                chunkWidth.text = "4";
            if (chunkLength.text == "")
                chunkLength.text = "4";

            float x_size_parse = Mathf.Clamp(float.Parse(chunkWidth.text), 1f, 8f);
            float z_size_parse = Mathf.Clamp(float.Parse(chunkWidth.text), 1f, 8f);

            x_size = (int)x_size_parse * 32;
            z_size = (int)z_size_parse * 32;

            if (lowAmp.text == "")
                lowAmp.text = "30";
            if (medAmp.text == "")
                medAmp.text = "10";
            if (highAmp.text == "")
                highAmp.text = "1";
            if (treeDensity.text == "")
                treeDensity.text = "0";

            float rand1 = Random.Range(-10000, 10000);
            float rand2 = Random.Range(-10000, 10000);
            float rand3 = Random.Range(-10000, 10000);
            float rand4 = Random.Range(-10000, 10000);
            float rand5 = Random.Range(-10000, 10000);
            float rand6 = Random.Range(-10000, 10000);

            GenerateTrees.instance.ClearLists();
            int treeDensityNum = (int)Mathf.Clamp(float.Parse(treeDensity.text), 0f, 20f);

            for (int x = 0; x < x_size; x+= 32)
            {
                for(int z = 0; z < z_size; z+= 32)
                {
                    VoxelRender render = Instantiate(voxelMesh, GameObject.Find("VoxelMeshParent").transform).GetComponent<VoxelRender>();
                    render.GenerateVoxelMesh(float.Parse(lowAmp.text), float.Parse(medAmp.text), float.Parse(highAmp.text), x, z,
                        rand1, rand2, rand3, rand4, rand5, rand6, treeDensityNum);
                    render.EasyUpdateMesh();
                }
            }
            GenerateTrees.instance.UpdateMesh();

            int ID = 0;
            string path = Application.persistentDataPath + "/saves";
            foreach (string folder in System.IO.Directory.GetDirectories(path))
            {
                ID += 1;
            }
            string FolderName = "world_" + ID.ToString();
            Debug.Log(FolderName);
            while (Directory.Exists(Application.persistentDataPath + "/saves/" + FolderName)) //Make sure that if a save already exists with the same name, a unique save is created.
            {
                ID += 1;
                FolderName = "world_" + ID.ToString();
            }
            if (!Directory.Exists(Application.persistentDataPath + "/saves/" + FolderName))
            {
                //if save folder doesn't exist, create it
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + FolderName);
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + FolderName + "/mesh");
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + FolderName + "/trees");
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/" + FolderName + "/gameobjects");
            }
            SL.SetSaveID(FolderName);
            SL.SaveWorld();
        }
        else
        {
            SL.SetSaveID("world_" + (worldSelect.GetSelection()-1).ToString());
            SL.LoadSave();
        }
        CanvasMenu.GetComponent<Canvas>().enabled = false;
        CanvasPersistent.GetComponent<Canvas>().enabled = true;
        SetSteveStart();
    }
}
