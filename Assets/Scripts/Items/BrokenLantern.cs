using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLantern : MonoBehaviour
{
    public float forceToBreak = 8f;
    public float delayAfterBreak = 5f;
    public float delayLight=3f;
    public GameObject[] parts;

    void OnCollisionEnter(Collision other){
        if (other.relativeVelocity.magnitude > forceToBreak){
            Debug.Log("big impact");
            Impact();
        }
    }

    void Impact(){
        StartCoroutine(PutOutLight());
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
        Transform fire = GetComponentInChildren<ParticleSystem>().transform;
        Light light = GetComponentInChildren<Light>();
        Vector3 s = fire.localScale / delayLight;
        float i = light.intensity / delayLight;
        Debug.Log("Light-fire"+ s + " "+i);
        float timer = 0;
        while (timer<delayLight)
        {
            timer += Time.deltaTime;
            fire.localScale -= s * Time.deltaTime;
            light.intensity -= i * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
