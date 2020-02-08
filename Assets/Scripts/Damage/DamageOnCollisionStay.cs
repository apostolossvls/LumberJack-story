using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollisionStay : MonoBehaviour
{
    public float damage=1f;
    public float TicksPerSecond=1f;
    public List<Transform> targets;
    List<float> tickTimer;
    public string[] tags;
    public string[] layers;

    void Start()
    {
        targets = new List<Transform>{};
        tickTimer = new List<float>{};
    }

    void Update(){
        List<int> indexes = new List<int>{};
        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i]){
                indexes.Add(OnArray(targets[i]));
            }
        }
        foreach (int i in indexes)
        {
            targets.RemoveAt(i);
            tickTimer.RemoveAt(i);
        }
    }

    void DealDamage(Transform t){
        Health h = t.gameObject.GetComponent<Health>();
        if (h){
            h.OnDamage(damage);
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (this.enabled){
            if (TicksPerSecond!=0){
                if ((TagMatch(other.transform.tag) || LayerMatch(LayerMask.LayerToName(other.gameObject.layer))) && OnArray(other.transform)==-1){
                    targets.Add(other.transform);
                    tickTimer.Add(TicksPerSecond);
                }
                for (int i = 0; i < tickTimer.Count; i++)
                {
                    tickTimer[i] += Time.deltaTime;
                    if (tickTimer[i]>=TicksPerSecond){
                        tickTimer[i]=0;
                        DealDamage(targets[i]);
                    }
                }
            }
            else
            {
                DealDamage(other.transform);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (this.enabled){
            if (OnArray(other.transform)!=-1){
                int index = OnArray(other.transform);
                targets.RemoveAt(index);
                tickTimer.RemoveAt(index);
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
    

    int OnArray(Transform t){
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i]==t) return i;
        }
        return -1;
    }
}
