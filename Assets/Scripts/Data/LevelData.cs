using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelData
{
    public string buildName;
    public int buildIndex;
    public bool cleared;
    public List<int> humanDeaths;
    public List<int> dogDeaths;

    public LevelData(int index, bool clear, List<int> hDeaths, List<int> dDeaths) {
        buildIndex = index;
        buildName = SceneManager.GetSceneByBuildIndex(buildIndex).name;
        cleared = clear;
        humanDeaths = hDeaths;
        dogDeaths = dDeaths;
        //Debug.Log("buildName: "+buildName+" - buildIndex: "+buildIndex+" - cleared: "+cleared);
    }
}
