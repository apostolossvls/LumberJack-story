using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float power=1f;

    void Update(){
        if (power<=0){
            PutOut();
        }
    }

    public void SetPower(float amount, bool fixedNumber=false){
        if (!fixedNumber){
            power += amount;
        }
        else {
            power = amount;
        }
    }

    public void PutOut(){
        Destroy(gameObject);
    }

}
