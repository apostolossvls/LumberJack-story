using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //level
    public int[] levelIndexes;
    public bool[] levelCleared;
    public List<int>[] levelHumanDeaths;
    public List<int>[] levelDogDeaths;

    //audio
    public bool[] audioMutes;
    public float[] audioVolumes;

    //quality
    public int qualityIndex;
    public bool qualityShowFPS;

    //Gameplay
    public bool gameplayShowInteractIndicator;
    public bool gameplayShowHintIndicator;
    public bool gameplayShowDogVision;

    //Controls
    public string[,,] controlsInputPaths = new string[2,32,4];
    public string[,,] controlsInputID = new string[2,32,4];

    public GameData(LevelDataManager lm, PauseMenuManager pm) {
        //level
        levelIndexes = lm.GetLevelsIndexes();
        levelCleared = lm.GetLevelsCleared();
        levelHumanDeaths = lm.GetLevelsHumanDeaths();
        levelDogDeaths = lm.GetLevelsDogDeaths();
        //audio
        audioMutes = pm.AudioMute;
        audioVolumes = pm.Volumes;
        //quality
        qualityIndex = pm.qualityIndex;
        qualityShowFPS = pm.isShowFPS;
        //gameplay
        gameplayShowInteractIndicator = pm.showInteractIndicator;
        gameplayShowHintIndicator = pm.showHintIndicator;
        gameplayShowDogVision = pm.showDogVision;
        //controls
        controlsInputPaths = pm.controlsPaths;
        controlsInputID = pm.bindingsID;
    }
}
