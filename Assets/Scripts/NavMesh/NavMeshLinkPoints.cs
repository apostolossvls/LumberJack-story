using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshLinkPoints : MonoBehaviour
{
    NavMeshLink link;
    public Transform start;
    public Transform end;
    public bool automatic = true;

    void Start()
    {
        link = GetComponent<NavMeshLink>();
    }

    void Update()
    {
        if (automatic) {
            AlighPoints();
        }
    }

    public void AlighPoints(){
        if (link && start && end) {
            link.startPoint = start.position - transform.position;
            link.endPoint = end.position - transform.position;
        }
    }
}
