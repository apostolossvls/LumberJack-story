using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCollider : MonoBehaviour
{
    public List<Transform> possibleinteracts;
    public string[] tags;

    void Start(){
        possibleinteracts = new List<Transform>{};
    }

    void Update(){
        List<int> indexes = new List<int>{};
        foreach (Transform t in possibleinteracts)
        {
            if (!TagMatch(t.tag)) indexes.Add(OnArray(t));
        }
        foreach (int i in indexes)
        {
            possibleinteracts.RemoveAt(i);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (this.enabled){
            if (TagMatch(other.tag) && OnArray(other.transform)==-1 && ComponentMatchWithTag(other.tag)){
                //small test (if on other playable cahracter then get only if is on the right hand)
                if (!other.GetComponentInParent<InteractControl>())
                    possibleinteracts.Add(other.transform);
                else {
                    InteractControl i = other.GetComponentInParent<InteractControl>();
                    if (i.rightGrab == other.transform || (i.leftGrab == other.transform && !i.rightGrab))
                    possibleinteracts.Add(other.transform);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (this.enabled){
            if (OnArray(other.transform)!=-1){
                possibleinteracts.RemoveAt(OnArray(other.transform));
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
        else if (t=="HumanInteractable"){
            return true;
        }
        else return false;
    }

    int OnArray(Transform t){
        for (int i = 0; i < possibleinteracts.Count; i++)
        {
            if (possibleinteracts[i]==t) return i;
        }
        return -1;
    }
}
