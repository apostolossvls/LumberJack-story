using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static LevelDataManager instance;
    public LevelData[] levelDatas;
    public int levelIndex;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    //  GET

    public int[] GetLevelsIndexes(){
        int[] array = new int[levelDatas.Length];
        for (int i = 0; i < levelDatas.Length; i++)
        {
            array[i] = levelDatas[i].buildIndex;
        }
        return array;
    }

    public bool[] GetLevelsCleared(){
        bool[] array = new bool[levelDatas.Length];
        for (int i = 0; i < levelDatas.Length; i++)
        {
            array[i] = levelDatas[i].cleared;
        }
        return array;
    }

    public string[] GetLevelsNames(){
        string[] array = new string[levelDatas.Length];
        for (int i = 0; i < levelDatas.Length; i++)
        {
            array[i] = levelDatas[i].buildName;
        }
        return array;
    }

    //  SET
    public void SetLevelsAll(int[] indexes, bool[] clears){
        List<LevelData> list = new List<LevelData>{};
        for (int i = 0; i < indexes.Length; i++)
        {
            list.Add(new LevelData(indexes[i], clears[i]));
            //levelDatas[i].buildIndex = indexes[i];
            //levelDatas[i].cleared = clears[i]; 
        }
        levelDatas = list.ToArray();
        //Debug.Log("Length: "+levelDatas.Length);
    }

    public void LevelCompleted(){
        if (levelIndex != -1) {
            for (int i = 0; i < levelDatas.Length; i++)
            {
                if (levelDatas[i].buildIndex == levelIndex){
                    levelDatas[i].cleared = true;
                }
            }
        }
    }
}
