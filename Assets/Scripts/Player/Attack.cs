using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float duration = 0.5f;
    public float cooldown = 0f;
    public GameObject attackCollider;
    bool attacking;

    void Start()
    {
        attacking = false;
    }

    void Update()
    {
        if (Input.GetButtonUp("Action") && !attacking){
            StartCoroutine(AttackAction());
        }
    }

    public IEnumerator AttackAction(){
        attacking = true;
        attackCollider.SetActive(true);
        //animation
        yield return new WaitForSeconds(duration);
        attackCollider.SetActive(false);
        yield return new WaitForSeconds(cooldown);
        attacking = false;
        yield return null;
    }
}
