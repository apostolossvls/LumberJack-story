using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        if (other.tag=="Fire"){
            Fire fire = other.GetComponent<Fire>();
            if (fire){
                fire.SetPower(-Time.deltaTime*10f);
            }
        }
    }
    /*void OnTriggerStay(Collider other){
        if (other.tag=="Fire"){
            Debug.Log("water found fire");
            Fire fire = other.GetComponent<Fire>();
            if (fire){
                fire.SetPower(-Time.deltaTime*10f);
                Debug.Log("water found fire script");
            }
        }
    }*/
}
