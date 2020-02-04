using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentDisabler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Collider col;
    bool triggered;
    bool activated;

    void Start(){
        triggered = false;
        activated = false;
    }

    /*void OnTriggerStay(Collider other){
        if (other.tag=="PlayerDog"){
            Debug.Log("playerdogtrigger");
            NavMeshAgent a = other.GetComponent<NavMeshAgent>();
            if (a) agent = a;
        }
    }
    void OnTriggerExit(Collider other){
        if (other.tag=="PlayerDog"){
            if (agent == other.GetComponent<NavMeshAgent>()){
                agent = null;
            }
        }
    }*/

    public void SetAgent(bool active){
        if (!triggered) triggered = true;
        else if (active == activated) return;
        activated = active;
        //Debug.Log("SetAgent: "+name);
        /*if (active){
            NavMeshLinkPoints s = GetComponentInChildren<NavMeshLinkPoints>();
            if (s) {
                s.AlighPoints();
            }
        }*/
        NavMeshAgent agent = GameObject.FindGameObjectWithTag("PlayerDog").GetComponent<NavMeshAgent>();
        if (agent){
            if (!col) return;
            if (col.bounds.Contains(agent.transform.position)){
                //Debug.Log("disabler has agent");
                //NavMeshSurface s = GetComponent<NavMeshSurface>();
                //s.BuildNavMesh();
                agent.enabled = active;
                NavMeshMovement movement = agent.GetComponent<NavMeshMovement>();
                if (movement) movement.enabled = active;
            }
        }
    }

    public IEnumerator UpdateSurface(float sec){
        NavMeshSurface s = GetComponent<NavMeshSurface>();
        float timer = 0;
        while(timer < sec){
            timer += Time.deltaTime;
            Debug.Log("surface update");
            s.BuildNavMesh();

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
