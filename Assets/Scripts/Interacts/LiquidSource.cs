using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSource : MonoBehaviour
{
    public string liquidName;
    public GameObject[] contentPrefab; //prefab as how the liquid will look inside the container. 0: bucket
    public GameObject[] liquidPrefab;

    bool GetLiquid(Transform t){
        bool flag = false;
        InteractControl i = t.GetComponent<InteractControl>();
        if (i){
            if (i.IsHuman){
                if (i.rightGrab){
                Bucket b = i.rightGrab.GetComponentInChildren<Bucket>();
                    if (b){
                        flag = true;
                        b.FillBucketWith(liquidName, contentPrefab[0], liquidPrefab[0]);
                    }
                }
            }
        } 
        return flag;
    }

    void OnHoldInteract(MessageArgs msg)
    {
        msg.received = GetLiquid(msg.sender);

    }
}
