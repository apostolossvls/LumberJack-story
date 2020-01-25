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

            Vector3 throwingAngle = i.throwingAngle;
            float throwPower = i.throwPower;
            float throwForce = i.throwForce;
            Vector3 v = new Vector3(throwingAngle.z, throwingAngle.y, throwingAngle.x);
            GameObject g = GameObject.Instantiate(liquidPrefab, transform.position, Quaternion.LookRotation(v));
            //g.transform.parent = transform;
            //g.transform.rotation = Quaternion.LookRotation(v);
            ParticleSystem ps = g.GetComponentInChildren<ParticleSystem>();
            if (ps){
                var sh = ps.shape;
                sh.rotation = Quaternion.LookRotation(throwingAngle).eulerAngles;
                Debug.DrawRay(transform.position, throwingAngle, Color.blue, 3f);
                //sh.rotation = new Vector3(0 , sh.rotation.y, 0);
                ps.Play();
            }
            g.GetComponent<Rigidbody>().AddForce(throwingAngle * throwPower * throwForce, ForceMode.Impulse);
            Destroy(g, 3.5f);

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
