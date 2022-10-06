using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
public class StartGame : MonoBehaviour
{
    Button butt;
    private Quaternion light_rot;
    public GameObject directionalLight;
    public GameObject CanvasMenu;
    public GameObject CanvasPersistent;
    public VoxelRender voxelRender;
    public WorldSelect worldSelect;
    public SaveLoad SL;
    public TextMeshProUGUI saveInfo;
    public TMP_InputField lowAmp;
    public TMP_InputField medAmp;
    public TMP_InputField highAmp;

    // Start is called before the first frame update
    void Start()
    {
        lowAmp = GameObject.Find("LowAmp").GetComponent<TMP_InputField>();
        medAmp = GameObject.Find("MedAmp").GetComponent<TMP_InputField>();
        highAmp = GameObject.Find("HighAmp").GetComponent<TMP_InputField>();
        saveInfo = GameObject.Find("SaveInfo").GetComponent<TextMeshProUGUI>();
        directionalLight = GameObject.Find("Directional Light");
        light_rot = directionalLight.transform.localRotation;
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
        CanvasMenu = GameObject.Find("CanvasMenu");
        CanvasPersistent = GameObject.Find("CanvasPersistent");
        voxelRender = GameObject.Find("VoxelMesh").GetComponent<VoxelRender>();
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
        directionalLight.transform.localRotation = light_rot;
        Cursor.lockState = CursorLockMode.Locked;
        print(lowAmp.text);
        print(medAmp.text);
        print(highAmp.text);
        if (worldSelect.GetSelection() == 0)
        {
            voxelRender.GenerateVoxelMesh();
            voxelRender.UpdateMesh();
            voxelRender.GT.UpdateMesh();
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
