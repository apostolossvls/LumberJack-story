using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSceneCollider : MonoBehaviour
{
    public int SceneIndex;
    public string[] tags;
    public string[] layers;

    void OnTriggerEnter(Collider other)
    {
        if ((TagMatch(other.transform.tag) || LayerMatch(LayerMask.LayerToName(other.gameObject.layer)))){
            LevelSettings.LoadSceneIndex(SceneIndex);
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
