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
    }

    void Update(){
        if (InspectorTriggerSave){
            InspectorTriggerSave = false;
            SaveFile();
        }
        if (InspectorTriggerLoad){
            InspectorTriggerLoad = false;
            LoadFile();
        }
        if (InspectorTriggerLevel){
            InspectorTriggerLevel = false;
            levelDataManager.levelDatas[TestIndex].cleared = TestBool;
        }
    }

    public void SaveFile()
    {
        string destination = m_Path + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        GameData data = new GameData(levelDataManager.GetLevelsIndexes(), levelDataManager.GetLevelsCleared());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = m_Path + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        //  TEST
        LevelDataManager.instance.SetLevelsAll(data.levelIndexes, data.levelCleared);
    }
}
