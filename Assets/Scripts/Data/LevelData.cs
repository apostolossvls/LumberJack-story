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

    public LevelData(int index, bool clear) {
        buildIndex = index;
        buildName = SceneManager.GetSceneByBuildIndex(buildIndex).name;
        cleared = clear;
        Debug.Log("buildName: "+buildName+" - buildIndex: "+buildIndex+" - cleared: "+cleared);
    }
}
