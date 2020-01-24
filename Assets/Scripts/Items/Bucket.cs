using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    bool filled;
    public string liquidName;
    public GameObject contentPrefab;
    public GameObject liquidPrefab;
    public Transform contentParent;

    void Start(){
        filled = false;
        liquidName = null;
        contentPrefab = null;
        liquidPrefab = null;
    }

    bool ThrowContent(Transform t){
        bool flag = false;
        InteractControl i = t.GetComponent<InteractControl>();
        if (filled && i.IsHuman){
            filled = false;
            flag = true;

            GameObject g = GameObject.Instantiate(liquidPrefab, transform.position, Quaternion.identity);
            Vector3 throwingAngle = i.throwingAngle;
            float throwPower = i.throwPower;
            float throwForce = i.throwForce;
            g.GetComponent<Rigidbody>().AddForce(throwingAngle * throwPower * throwForce, ForceMode.Impulse);

            liquidName = null;
            SetContent(null);
            liquidPrefab = null;
        }
        return flag;
    }

    public void FillBucketWith(string n, GameObject c, GameObject l){
        filled = true;
        liquidName = n;
        SetContent(c);
        liquidPrefab = l;
    }

    void SetContent(GameObject c){
        if (contentParent.childCount>0){
            Destroy(contentParent.GetChild(0).gameObject);
        }
        contentPrefab = c;
        if (c!=null){
            GameObject g = GameObject.Instantiate(c, contentParent.position, contentParent.rotation);
            g.transform.SetParent(contentParent);
        }
    }

    void OnThrow(MessageArgs msg){
        msg.received = ThrowContent(msg.sender);
    }
}
