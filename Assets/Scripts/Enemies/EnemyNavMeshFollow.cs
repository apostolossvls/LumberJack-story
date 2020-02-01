using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshFollow : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform target;
    public float sphereRadius = 0.5f;
    public float viewDistance = 5f;
    public string[] tags;
    public LayerMask layerMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target) {
            agent.SetDestination(target.position);
        }
    }

    void OnTriggerEnter(Collider other){
        if (TagMatch(other.tag) && !target){
            target = other.transform;
        }
    }
    void OnTriggerExit(Collider other){
        if (target){
            if (target==other.transform){
                target = null;
            }
        }
    }

    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }
}
