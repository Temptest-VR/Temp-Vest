using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionDestroyAnimation : MonoBehaviour
{
    [SerializeField] private float destroyAnimationDuration = 2f;
    [SerializeField] private bool nextPassThroughState = false;
    [SerializeField] private SpawnDespawnTransition spawnDespawnTransition;
    [SerializeField] private Material defaultMaterial;
    private bool destroyStarted = false;
    private float time = 0f;
    private MeshRenderer meshRenderer;
    private Material material;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SelectionDestroyTrigger()
    {
        destroyStarted = true;
        time = 0f;
        material = meshRenderer.material;
        GameManager.instance.PasssthhroughToggle(nextPassThroughState);
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
                spawnDespawnTransition.TransitionToggleOn();
                spawnDespawnTransition.TransitionToggleOff();
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
                // GameManager.instance.NextTransition();
                // GameManager.instance.PasssthhroughToggle(nextPassThroughState);
                destroyStarted = false;
                transform.position = GameManager.instance.mainCamera.transform.position + GameManager.instance.mainCamera.transform.forward * 0.7f + new Vector3(0, -0.2f, 0);
                transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                transform.rotation = Quaternion.Euler(0f, 45f, 45f);
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                meshRenderer.material = defaultMaterial;
                gameObject.SetActive(false);
                // Destroy(gameObject);
            }
        }
    }
}
