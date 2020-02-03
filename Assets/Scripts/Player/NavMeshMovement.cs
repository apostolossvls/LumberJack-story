using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public bool wantsToFollow=true;
    float lastPosFar;
    Vector3 lastPos;
    float pathPendingCounter;

    //UI
    public Transform talkImageParent;
    public Transform talkImage1;

    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve curve = new AnimationCurve ();

    void OnEnable(){
        activated=true;
        lastPosFar = 0;
        lastPos = transform.position;
        pathPendingCounter=0;
        StartCoroutine("DoStart");
        //NavMeshBuilder.UpdateNavMeshData();
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
        if (target && activated && wantsToFollow){
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            bool followError = false;
            if (follow) {
                agent.SetDestination(target.position+offset);
                float dist=agent.remainingDistance;

                if (dist!=Mathf.Infinity && agent.pathStatus==NavMeshPathStatus.PathComplete && agent.remainingDistance>agent.stoppingDistance){
                    //running to human
                }

                if (dist!=Mathf.Infinity && agent.pathPending){
                    pathPendingCounter+=Time.deltaTime;
                    followError = true;
                }
                else {
                    pathPendingCounter=0;
                }
                if (pathPendingCounter>4) {
                    pathPendingCounter=0;
                    StartCoroutine(CantReachTalk(true, false));
                    Debug.Log("pathProblem1");
                }

                if (agent.remainingDistance > agent.stoppingDistance){
                    if (Vector3.Distance(transform.position, lastPos) < 0.2f){
                        lastPosFar+=Time.deltaTime;
                        followError = true;
                    }
                    else {
                        lastPosFar = 0;
                    }
                    lastPos = transform.position;
                    if (lastPosFar > 8){
                        lastPosFar = 0;
                        follow = false;
                        StartCoroutine(CantReachTalk(true, false)); 
                        Debug.Log("pathProblem2");
                    }          
                }
            }
            else {
                if (Vector3.Distance(transform.position, target.position) < 5f){
                    lastPosFar = 0;
                    follow = true;
                }
                else{
                    lastPosFar+=Time.deltaTime;
                    if (lastPosFar > 4){
                        lastPosFar = 0;
                        follow = false;
                        StartCoroutine(CantReachTalk(true, true)); 
                        Debug.Log("pathProblem2");
                    }  
                }
            }
        }
        else { 
            agent.SetDestination(transform.position);
        }

    }

    IEnumerator CantReachTalk(bool confused, bool sad){
        Debug.Log("DOG CANT REACH");
        activated = false;
        talkImageParent.gameObject.SetActive(true);
        float timer = 0;
        while (timer<2){
            Vector3 c = Camera.main.WorldToViewportPoint(transform.position);
            float a = Vector3.Angle(transform.position, transform.position+c);
            Vector3 pos =  Camera.main.ScreenToWorldPoint(
                Camera.main.ViewportToScreenPoint(new Vector3(Mathf.Clamp(c.x, 0.1f, 0.9f), Mathf.Clamp(c.y, 0.1f, 0.9f), -Camera.main.transform.position.z)))
                + new Vector3 (0,2f,0) 
            ;
            talkImageParent.position = pos;
            //Debug.Log(a + ", c: "+c);
            talkImage1.rotation = Quaternion.Euler(talkImage1.rotation.x, talkImage1.rotation.y, Mathf.Rad2Deg * Mathf.Sign(c.x) *((transform.position - talkImage1.position).normalized.x));
            timer+=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(2f);
        activated = true;
        talkImageParent.gameObject.SetActive(false);
        yield return null;
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
