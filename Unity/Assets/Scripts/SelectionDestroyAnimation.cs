using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionDestroyAnimation : MonoBehaviour
{
    [SerializeField] private float destroyAnimationDuration = 2f;
    [SerializeField] private bool nextPassThroughState = false;
    private bool destroyStarted = false;
    private float time = 0f;
    private MeshRenderer meshRenderer;
    private Material material;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
    }

    public void SelectionDestroyTrigger()
    {
        destroyStarted = true;
    }

    private void Update()
    {
        if (destroyStarted)
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x - time / 1000f, transform.localScale.y - time / 1000f, transform.localScale.z - time / 1000f);
            material.SetColor("_BaseColor", new Color(material.color.r, material.color.g, material.color.b, (destroyAnimationDuration - time) / destroyAnimationDuration));
            if (destroyStarted && time >= destroyAnimationDuration)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
                GameManager.instance.NextTransition();
                GameManager.instance.PasssthhroughToggle(nextPassThroughState);
                Destroy(gameObject);
            }
        }
    }
}
