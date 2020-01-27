using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum OffMeshLinkMoveMethod {
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}

public class NavMeshMovement : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    NavMeshAgent agent;
    public float jumpDuration=1f;
    bool activated=true;
    public bool follow=true;
    int lastPosFar;
    Vector3 lastPos;
    int pathPendingCounter;

    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve curve = new AnimationCurve ();

    void OnEnable(){
        activated=true;
        lastPosFar = 0;
        lastPos = transform.position;
        pathPendingCounter=0;
        StartCoroutine("DoStart");
    }
    void OnDisable(){
        activated=false;
        StopCoroutine("DoStart");
    }


    IEnumerator DoStart () {
        agent = GetComponent<NavMeshAgent> ();
        agent.autoTraverseOffMeshLink = false;
        follow=true;
        while (activated && follow) {
            if (agent.isOnOffMeshLink) {
                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                yield return StartCoroutine (NormalSpeed (agent));
                else if (method == OffMeshLinkMoveMethod.Parabola)
                yield return StartCoroutine (Parabola (agent, 2.0f, agent.speed*jumpDuration));
                else if (method == OffMeshLinkMoveMethod.Curve)
                yield return StartCoroutine (Curve (agent, 0.5f));
                agent.CompleteOffMeshLink ();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed (NavMeshAgent agent) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up*agent.baseOffset;
        while (agent.transform.position != endPos) {
            agent.transform.position = Vector3.MoveTowards (agent.transform.position, endPos, agent.speed*Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator Parabola (NavMeshAgent agent, float height, float duration) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up*agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f) {
            float yOffset = height * 4.0f*(normalizedTime - normalizedTime*normalizedTime);
            agent.transform.position = Vector3.Lerp (startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
    IEnumerator Curve (NavMeshAgent agent, float duration) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up*agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f) {
            float yOffset = curve.Evaluate (normalizedTime);
            agent.transform.position = Vector3.Lerp (startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    void Update()
    {
        if (target && activated){
            agent.SetDestination(target.position+offset);
        }
        float dist=agent.remainingDistance;
        if (dist!=Mathf.Infinity && agent.pathStatus==NavMeshPathStatus.PathComplete && agent.remainingDistance>agent.stoppingDistance){
            //running to human
        }

        if (dist!=Mathf.Infinity && agent.pathPending && follow){
            //Debug.Log("path2");
            pathPendingCounter++;
        }
        else {
            pathPendingCounter=0;
        }
        if (pathPendingCounter>100) {
            CantReachBark();
            Debug.Log("pathProblem1");
        }

        if (follow && agent.remainingDistance > agent.stoppingDistance){
            if (Vector3.Distance(transform.position, lastPos) < 0.2f){
                lastPosFar++;
            }
            else {
                lastPosFar = 0;
            }
            lastPos = transform.position;
            if (lastPosFar > 100){
                CantReachBark();
                Debug.Log("pathProblem2");
            }          
        }
    }

    void CantReachBark(){
        Debug.Log("DOG CANT REACH");
        //bark
    }

    /*
    public Transform target;
    NavMeshAgent agent;
    bool MoveAcrossNavMeshesStarted;

    void Start()
    {
        MoveAcrossNavMeshesStarted=false;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted){
            StartCoroutine(MoveAcrossNavMeshLink());
            MoveAcrossNavMeshesStarted=true;
        }

        if (target){
            agent.SetDestination(target.position);
        }
    }

    IEnumerator MoveAcrossNavMeshLink()
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        agent.updateRotation = false;

        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float duration = (endPos-startPos).magnitude/agent.velocity.magnitude;
        float t = 0.0f;
        float tStep = 1.0f/duration;
        while(t<1.0f){
            transform.position = Vector3.Lerp(startPos,endPos,t);
            agent.destination = transform.position;
            t+=tStep*Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
        agent.updateRotation = true;
        agent.CompleteOffMeshLink();
        MoveAcrossNavMeshesStarted= false;
    }
    */
}
