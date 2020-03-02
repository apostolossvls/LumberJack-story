using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        SaveJson(InputManager.controls.asset.ToJson());
        InputManager.controls.asset.ToJson();
    }

    public void Load(){
        GameData data = LoadFile();
        string inputs = LoadJson();
        
        if (data != null) {
            SetValues(data, inputs);
        }
    }

    public void SetValues(GameData data, string inputs){
        //level
        if (LevelDataManager.instance) LevelDataManager.instance.SetLevelsAll(data);
        //pauseMenu
        if (PauseMenuManager.instance) PauseMenuManager.instance.SetAll(data);
        if (InputManager.controls.asset != null && inputs !=null && inputs!="") InputManager.controls.asset.LoadFromJson(inputs);
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

    void SaveJson(string data){
        string destination = m_Path + "/input.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
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

    string LoadJson()
    {
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

        return data;
    }
}
