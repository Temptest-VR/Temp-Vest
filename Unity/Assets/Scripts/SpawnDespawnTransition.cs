using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class SpawnDespawnTransition : MonoBehaviour
{
    public List<Transform> toggleObjectsOn = new List<Transform>();
    public List<Transform> toggleObjectsOff = new List<Transform>();
    public List<GrabInteractable> grabbables = new List<GrabInteractable>();
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

        if (grabbables.Count > 0)
        {
            foreach (var grabbable in grabbables)
            {
                grabbable.Enable();
            }
        }
    }
}
