using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    public bool active = true;
    void OnEnable()
    {
        IsActive();
    }

    public void IsActive(){
        if (!active) gameObject.SetActive(false);
    }
}
