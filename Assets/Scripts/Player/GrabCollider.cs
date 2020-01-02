using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCollider : MonoBehaviour
{
    public List<Transform> possibleGrabs;
    public string[] tags;

    void Start(){
        possibleGrabs = new List<Transform>{};
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.enabled){
            if (TagMatch(other.tag) && OnArray(other.transform)==-1 && ComponentMatchWithTag(other.tag)){
                possibleGrabs.Add(other.transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (this.enabled){
            if (OnArray(other.transform)!=-1){
                possibleGrabs.RemoveAt(OnArray(other.transform));
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

    bool ComponentMatchWithTag(string t){
        if (t=="Item"){
            return true;
        }
        else if (t=="Grabbable"){
            return true;
        }
        else if (t=="Draggable"){
            return true;
        }
        else return false;
    }

    int OnArray(Transform t){
        for (int i = 0; i < possibleGrabs.Count; i++)
        {
            if (possibleGrabs[i]==t) return i;
        }
        return -1;
    }
}
