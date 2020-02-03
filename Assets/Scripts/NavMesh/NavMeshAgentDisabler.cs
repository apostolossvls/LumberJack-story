using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentDisabler : MonoBehaviour
{
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.tag=="PlayerDog"){
            agent = other.GetComponent<NavMeshAgent>();
        }
    }
    void OnTriggerExit(Collider other){
        if (other.tag=="PlayerDog"){
            if (agent == other.GetComponent<NavMeshAgent>()){
                agent = null;
            }
        }
    }

    public void SetAgent(bool active){
        if (agent){
            NavMeshMovement movement = agent.GetComponent<NavMeshMovement>();
            if (movement) movement.enabled = active;
        }
    }
}
