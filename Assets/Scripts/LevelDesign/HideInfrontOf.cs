using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInfrontOf : MonoBehaviour
{
    public string[] tags;
    public bool hidden;
    Renderer[] renderers;
    int counter;

    void Start(){
        renderers = GetComponentsInChildren<Renderer>();
        counter = 0;
    }

    void OnTriggerEnter(Collider other){
        if (TagMatch(other.tag)){
            counter++;
            Hide();
        }
    }
    void OnTriggerExit(Collider other){
        if (TagMatch(other.tag)){
            counter--;
            Hide();
        }
    }   

    void Hide(){
        if (counter<0) counter = 0;
        foreach (Renderer r in renderers)
        {
            r.enabled = counter > 0 ? false : true;
        }
    }

    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }
}
