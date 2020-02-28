﻿using System.Collections;
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

    public List<int>[] GetLevelsHumanDeaths(){
        List<int>[] matrix = new List<int>[levelDatas.Length];
        for (int i = 0; i < levelDatas.Length; i++)
        {
            matrix[i] = levelDatas[i].humanDeaths;
        }
        return matrix;
    }
    public List<int>[] GetLevelsDogDeaths(){
        List<int>[] matrix = new List<int>[levelDatas.Length];
        for (int i = 0; i < levelDatas.Length; i++)
        {
            matrix[i] = levelDatas[i].dogDeaths;
        }
        return matrix;
    }

    //  SET
    public void SetLevelsAll(GameData data){
        if (data!=null){
            List<LevelData> list = new List<LevelData>{};
            for (int i = 0; i < data.levelIndexes.Length; i++)
            {
                list.Add(new LevelData(data.levelIndexes[i], 
                data.levelCleared[i], 
                data.levelHumanDeaths[i],
                data.levelDogDeaths[i]
                ));
                //levelDatas[i].buildIndex = indexes[i];
                //levelDatas[i].cleared = clears[i]; 
            }
            levelDatas = list.ToArray();
            //Debug.Log("Length: "+levelDatas.Length)
        }
    }

    public void SetLevelInfo(LevelSettings l){
        LevelWithBuildIndex(levelIndex).humanDeaths.Add(l.HumanDeaths);
        LevelWithBuildIndex(levelIndex).dogDeaths.Add(l.DogDeaths);
    }

    public void LevelCompleted(){
        LevelWithBuildIndex(levelIndex).cleared = true;
    }

    public LevelData LevelWithBuildIndex (int buildIndex){
        if (buildIndex != -1) {
            for (int i = 0; i < levelDatas.Length; i++)
            {
                if (levelDatas[i].buildIndex == buildIndex){
                    return levelDatas[i];
                }
            }
        }
        return null;
    }
}
