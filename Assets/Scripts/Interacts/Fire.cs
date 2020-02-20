using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float power=1f;
    public GameObject fire;
    public bool onFire = true;

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
        fire.SetActive(false);
        onFire = false;
    }

    public void LightUp(){
        fire.SetActive(true);
        onFire = true;
        power = 1f;
    }


    void OnTriggerStay(Collider other){
        LiquidSource lq = other.GetComponentInChildren<LiquidSource>();
        if (lq){
            if (lq.liquidName == "Water"){
                SetPower(0, true);
            }
        }
        Fire f = other.GetComponentInChildren<Fire>();
        if (f) {
            if (f.onFire && !onFire){
                LightUp();
            }
        }
    }
}
