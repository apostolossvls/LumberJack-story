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
}
