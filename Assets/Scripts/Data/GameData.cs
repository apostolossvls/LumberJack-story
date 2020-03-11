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
    public List<float>[] levelClearTimes; 

    //audio
    public bool[] audioMutes;
    public float[] audioVolumes;

    //quality
    public int qualityIndex;
    public int qualityResolutionIndex;
    public bool qualityIsFullscreen;
    public bool qualityShowFPS;
    public int qualityAntiAliasingIndex;
    public int qualityVSyncIndex;

    //Gameplay
    public bool gameplayShowInteractIndicator;
    public bool gameplayShowHintIndicator;
    public bool gameplayShowDogVision;
    public int gameplaylanguageIndex;

    //Controls
    public List<string> controlsInputPaths = new List<string>(){};
    public List<string>  controlsInputID = new List<string>(){};

    public GameData(LevelDataManager lm, PauseMenuManager pm) {
        //level
        levelIndexes = lm.GetLevelsIndexes();
        levelCleared = lm.GetLevelsCleared();
        levelHumanDeaths = lm.GetLevelsHumanDeaths();
        levelDogDeaths = lm.GetLevelsDogDeaths();
        levelClearTimes = lm.GetLevelsClearTimes();
        //audio
        audioMutes = pm.AudioMute;
        audioVolumes = pm.Volumes;
        //quality
        qualityIndex = pm.qualityIndex;
        qualityResolutionIndex = pm.resolutionIndex;
        qualityIsFullscreen = pm.isFullscreen;
        qualityShowFPS = pm.isShowFPS;
        qualityAntiAliasingIndex = pm.antiAliasingIndex;
        qualityVSyncIndex = pm.vSyncIndex;
        //gameplay
        gameplayShowInteractIndicator = pm.showInteractIndicator;
        gameplayShowHintIndicator = pm.showHintIndicator;
        gameplayShowDogVision = pm.showDogVision;
        //controls
        controlsInputPaths = pm.controlsPaths;
        controlsInputID = pm.controlsID;
        gameplaylanguageIndex = pm.languageIndex;
    }
}
