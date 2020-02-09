using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeJointTarget : MonoBehaviour
{
    public Transform target;
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.position);
    }
}
