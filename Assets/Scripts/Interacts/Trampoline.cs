using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float loadTime=1f;
    public float power = 10f;
    public float radius = 5f;
    public string[] tags;
    public Transform deformObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (TagMatch(other.tag)){
            StartCoroutine(Activate());
        }
    }

    IEnumerator Activate(){
        //LOADING
        Deform();
        yield return new WaitForSeconds(loadTime);
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (Collider col in hitColliders)
        {
            if (TagMatch(col.tag)){
                Push(col.transform);
            }
        }
        ResetForm();
        yield return null;
    }

    void Push(Transform t){
        Debug.Log("Pushing: "+t.name);
        Rigidbody rig = t.GetComponent<Rigidbody>();
        if (rig){
            rig.AddExplosionForce(power, transform.position, radius, 3.0F, ForceMode.Impulse);
        }
    }

    void Deform(){
        Debug.Log("Deform");
        deformObject.localScale = new Vector3(deformObject.localScale.x, deformObject.localScale.y/2, deformObject.localScale.z);
    }

    void ResetForm(){
        Debug.Log("ResetForm");
        deformObject.localScale = new Vector3(deformObject.localScale.x, deformObject.localScale.y*2, deformObject.localScale.z);
    }

    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }
}
