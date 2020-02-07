using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToCheckpoint : MonoBehaviour
{
    LevelSettings level;
    public string[] tags;
    public string[] layers;

    void Start(){
        level = LevelSettings.instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((TagMatch(other.transform.tag) || LayerMatch(other.gameObject.layer.ToString()))){
            if (Checkpoint3.LastCheckpoint) {
                Checkpoint3.LastCheckpoint.ResetOnCheckpoint();
            }
            else {
                LevelSettings.RestartScene();
            }
        }
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
