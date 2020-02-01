using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpAttack : MonoBehaviour
{
    Rigidbody rig;
    public Transform target;
    public string[] tags;
    public float jumpDelay = 1f;
    public float force = 10f;
    float timer;

    void Start(){
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (target) {

        }
    }

    void OnTriggerStay(Collider other){
        if (TagMatch(other.tag) && !target){
            timer = 0;
            target = other.transform;
        }
        if (target){
            if (IsGrounded()){
                timer += Time.deltaTime; 
            }
            else {
                timer = 0;
            }
            if (timer >= jumpDelay){
                timer = 0;
                JumpAttack();
            }
        } 
        else{
            timer = 0;
        }
    }
    void OnTriggerExit(Collider other){
        if (target){
            if (target==other.transform){
                target = null;
                timer = 0;
            }
        }
    }

    void JumpAttack(){
        rig.AddForce((target.position - transform.position).normalized * force, ForceMode.Impulse);
    }

    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }

    bool IsGrounded() {
        if (rig.velocity.y==0) return true;
        else return false;
    }
}
