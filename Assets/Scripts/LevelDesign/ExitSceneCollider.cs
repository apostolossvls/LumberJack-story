using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSceneCollider : MonoBehaviour
{
    public int SceneIndex;
    public string[] tags;
    public string[] layers;
    public bool isGoal = true;

    void OnTriggerEnter(Collider other)
    {
        if ((TagMatch(other.transform.tag) || LayerMatch(LayerMask.LayerToName(other.gameObject.layer)))){
            Goal();
        }
    }

    public void Goal(){
        if (isGoal){
            LevelDataManager.instance.LevelCompleted();
        }
        LevelSettings.LoadSceneIndex(SceneIndex);
    }

    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }

    bool LayerMatch (string s){
        foreach (string l in layers)
        {
            if (s==l) return true;
        }
        return false;
    }
}
