using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public float power=1f;
    public bool isTrigger = false;

    void Update(){
        if (power<=0){
            Die();
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

    public void Die(){
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Fire"){
            SetPower(-1 * Time.deltaTime);
        }
    }
}
