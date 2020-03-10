using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [HideInInspector] public string m_Path;
    public LevelDataManager levelDataManager;
    public bool InspectorTriggerSave;
    public bool InspectorTriggerLoad;
    public bool InspectorTriggerLevel;
    public int TestIndex;
    public bool TestBool;

    void Awake(){
        if (instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        m_Path = Application.dataPath;
        Debug.Log("dataPath : " + m_Path);
        Load();
    }

    void Update(){
        if (InspectorTriggerSave){
            InspectorTriggerSave = false;
            Save();
        }
        if (InspectorTriggerLoad){
            InspectorTriggerLoad = false;
            Load();
        }
        if (InspectorTriggerLevel){
            InspectorTriggerLevel = false;
            levelDataManager.levelDatas[TestIndex].cleared = TestBool;
        }
    }

    public void Save(){
        GameData data = new GameData(LevelDataManager.instance, PauseMenuManager.instance);
        SaveFile(data);
        //SaveJson(InputManager.controls.asset.ToJson(), "/inputs.inputactions");
        //string map = InputActionMap.ToJson(InputManager.controls.asset.actionMaps);
        //SaveJson(map, "/inputsMaps.inputactions");
        /*
        for (int i = 0; i < InputManager.controls.controlSchemes.Count; i++)
        {
            Debug.Log("InputManager.controls.controlSchemes: "+InputManager.controls.controlSchemes.ToString());
            for (int j = 0; j < InputManager.controls.Player.Get().bindings.Count; j++)
            {
                Debug.Log("bindings ("+j+") : "+InputManager.controls.Player.Get().bindings[j]);
            }
        }
        */
        //InputManager.controls.asset.ToJson();
    }

    public void Load(){
        GameData data = LoadFile();
        //string inputs = LoadJson();
        
        if (data != null) {
            SetValues(data);
        }
    }

    public void SetValues(GameData data){
        //level
        if (LevelDataManager.instance) LevelDataManager.instance.SetLevelsAll(data);
        //pauseMenu
        //if (InputManager.controls.asset != null) LoadJson();
        if (PauseMenuManager.instance) PauseMenuManager.instance.SetAll(data);
    }

    void SaveFile(GameData data)
    {
        string destination = m_Path + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    void SaveJson(string jsonString, string path){
        //jsonString = "var asset = InputActionAsset.FromJson(@\"\n"+jsonString+"\");";
        string destination = m_Path + path;
        //File.WriteAllText(m_Path, jsonString);
        FileStream file = new FileStream(destination, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(file)){
            writer.Write(jsonString);
            writer.Close();
        }
        file.Close();
        /*
        string destination = m_Path + "/input.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
        */
    }

    GameData LoadFile()
    {
        string destination = m_Path + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogWarning("File not found (save.dat)");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        return data;
    }

    void LoadJson()
    {
        /*
        string destination = m_Path + "/inputs.inputactions";
        string jsonString = null;
        if(File.Exists(destination)){
            InputManager.controls.asset.LoadFromJson(File.ReadAllText(destination));
            InputActionMap.FromJson(File.ReadAllText(destination));
        }
        else
        {
            Debug.LogWarning("File not found (inputs.inputactions)");
            return null;
        }
        */
        string destination = m_Path + "/inputsMaps.inputactions";
        if(File.Exists(destination)){
            InputActionMap.FromJson(File.ReadAllText(destination));
            /*
            for (int i = 0; i < InputManager.controls.controlSchemes.Count; i++)
            {
                Debug.Log("InputManager.controls.controlSchemes: "+InputManager.controls.controlSchemes.ToString());
                for (int j = 0; j < InputManager.controls.Player.Get().bindings.Count; j++)
                {
                    Debug.Log("bindings ("+j+") : "+InputManager.controls.Player.Get().bindings[j]);
                }
            }
            */
        }
        else
        {
            Debug.LogWarning("File not found (inputsMaps.inputactions)");
        }
        //InputActionAsset.FromJson(jsonString);
        /*
        string destination = m_Path + "/input.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogWarning("File not found (input.dat)");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        string data = (string) bf.Deserialize(file);
        file.Close();
        */
    }
}
