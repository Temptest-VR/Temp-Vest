using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDespawnTransition : MonoBehaviour
{
    public List<Transform> toggleObjectsOn = new List<Transform>();
    public List<Transform> toggleObjectsOff = new List<Transform>();
    private Rigidbody rb;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    public void TransitionToggleOn()
    {
        foreach (var var in toggleObjectsOn)
        {
            var.gameObject.SetActive(true);
        }
    }

    public void TransitionToggleOff()
    {
        foreach (var var in toggleObjectsOff)
        {
            var.gameObject.SetActive(false);
        }
    }
}
