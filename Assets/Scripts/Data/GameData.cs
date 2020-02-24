using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[] levelIndexes;
    public bool[] levelCleared;

    public GameData(int[] t_levelIndexes, bool[] t_levelCleared) {
        levelIndexes = t_levelIndexes;
        levelCleared = t_levelCleared;
    }
}
