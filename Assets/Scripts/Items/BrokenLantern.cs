using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLantern : MonoBehaviour
{
    public float powerMod = 10f;
    public float forceToBreak = 8f;
    public float delayAfterBreak = 5f;
    public float delayLight=3f;
    public Transform fire;
    public Light fireLight;
    public Fire fireScript;
    public GameObject[] parts;

    void OnCollisionEnter(Collision other){
        if (other.relativeVelocity.magnitude > forceToBreak){
            //Debug.Log("big impact");
            Impact(other);
        }
    }

    void Impact(Collision collision){
        tag = "Untagged";
        fireScript.gameObject.SetActive(true);
        StartCoroutine(PutOutLight());
        StartCoroutine(Disassemble());
        foreach (GameObject part in parts)
        {
            Rigidbody r = part.GetComponent<Rigidbody>();
            if (!r) {
                r = part.AddComponent<Rigidbody>();
                r.mass = 0.1f;
            }
            else if (r.isKinematic){
                r.isKinematic = false;
            }
            part.GetComponent<Collider>().isTrigger = false;
            part.transform.SetParent(null);
            Destroy(part, delayAfterBreak);
        }
        Destroy(gameObject, delayAfterBreak);
    }

    IEnumerator PutOutLight(){
        fireScript.power = 1f;
        fire.localScale *= 2;
        fireLight.intensity *=1.5f;
        Vector3 s = fire.localScale / delayLight;
        float i = fireLight.intensity / delayLight;
        float f = fireScript.power / delayLight;
        //Debug.Log("Light-fire"+ s + " "+i);
        float timer = 0;
        while (timer<delayLight)
        {
            timer += Time.deltaTime;
            fire.localScale -= s * Time.deltaTime;
            fireLight.intensity -= i * Time.deltaTime;
            fireScript.power -= f * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    IEnumerator Disassemble(){
        yield return new WaitForSeconds(0.5f);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<Collider>());
        yield return null;
    }
}
