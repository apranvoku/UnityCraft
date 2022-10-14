using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
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
    public TMP_InputField lowAmp;
    public TMP_InputField medAmp;
    public TMP_InputField highAmp;

    // Start is called before the first frame update
    void Start()
    {
        x_size = 128;
        z_size = 128;
        playerController = GameObject.Find("Steve").GetComponent<PlayerController>();
        lowAmp = GameObject.Find("LowAmp").GetComponent<TMP_InputField>();
        medAmp = GameObject.Find("MedAmp").GetComponent<TMP_InputField>();
        highAmp = GameObject.Find("HighAmp").GetComponent<TMP_InputField>();
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            SL.SaveWorld();
            saveInfo.text = "Saved World!";
            StartCoroutine(ResetSaveInfo());
        }
    }

    public IEnumerator ResetSaveInfo()
    {
        yield return new WaitForSeconds(3f);
        saveInfo.text = "Press r to save...";
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
            if(lowAmp.text == "")
                lowAmp.text = "30";
            if (medAmp.text == "")
                medAmp.text = "10";
            if (highAmp.text == "")
                highAmp.text = "1";

            float rand1 = Random.Range(-10000, 10000);
            float rand2 = Random.Range(-10000, 10000);
            float rand3 = Random.Range(-10000, 10000);
            float rand4 = Random.Range(-10000, 10000);
            float rand5 = Random.Range(-10000, 10000);
            float rand6 = Random.Range(-10000, 10000);

            for (int x = 0; x < x_size; x+= 31)
            {
                for(int z = 0; z < z_size; z+= 31)
                {
                    VoxelRender render = Instantiate(voxelMesh).GetComponent<VoxelRender>();
                    render.GenerateVoxelMesh(float.Parse(lowAmp.text), float.Parse(medAmp.text), float.Parse(highAmp.text), x, z,
                        rand1, rand2, rand3, rand4, rand5, rand6);
                    render.UpdateMesh();
                    render.GT.UpdateMesh();
                }
            }
            //voxelRender.GenerateVoxelMesh(float.Parse(lowAmp.text), float.Parse(medAmp.text), float.Parse(highAmp.text));
            //voxelRender.UpdateMesh();
            //voxelRender.GT.UpdateMesh();
            int ID = 0;
            string path = Application.persistentDataPath + "/saves";
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                ID += 2;
            }
            SL.SetSaveID("world_" + (ID/2).ToString());
        }
        else
        {
            SL.SetSaveID("world_" + (worldSelect.GetSelection()-1).ToString());
            SL.LoadSave();
        }
        CanvasMenu.GetComponent<Canvas>().enabled = false;
        CanvasPersistent.GetComponent<Canvas>().enabled = true;
    }
}
