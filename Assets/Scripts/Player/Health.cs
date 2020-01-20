using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 1f;
    public float health;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (health<=0){
            Die();
        }
    }

    public void OnDamage(float dmg){
        health -= dmg;
    }

    void Die(){
        if (tag=="PlayerHuman" || tag=="PlayerDog")
        {
            LevelSettings.RestartScene();
        }
    }
}
