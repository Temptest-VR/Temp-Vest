using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform mainCamera;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private OVRPassthroughLayer layer;
    [SerializeField] private List<GameObject> transitionPrefabs = new List<GameObject>();
    private int transitionIndex = 0;
    public static GameManager instance;
    private float time = 0f;
    private float passThroughTransitionTime = 0f;
    private bool passThroughIsOn = true;
    private float passthrouToggleTransitionTime = 2f;
    private float audioLength = 0f;
    private IEnumerator transitionRoutine;
    private bool takeTransition = false;
    private bool firstTransition = true;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // NextTransition();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (passThroughTransitionTime + passthrouToggleTransitionTime > time)
        {
            if (passThroughIsOn)
            {
                layer.textureOpacity += 0.01f;
            }
            else
            {
                layer.textureOpacity -= 0.01f;
            }
        }
    }

    public void NextTransition()
    {
        if (transitionIndex == 0)
        {
            if (!firstTransition)
            {
                SpawnTransitionTriggers(transitionIndex);
            }
            else
            {
                firstTransition = false;
            }
            transitionIndex++;
            return;
        }
        transitionIndex++;
        if (transitionIndex >= transitionPrefabs.Count)
        {
            transitionIndex = 0;
            SpawnTransitionTriggers(transitionIndex);
            return;
        }
        audioManager.AudioTransition(transitionIndex);
        audioLength = audioManager.AudioLength(transitionIndex);
        StartCoroutine(TransitionRoutine(audioLength));
    }

    private void SpawnTransitionTriggers(int index)
    {
        Instantiate(transitionPrefabs[index], mainCamera.position + (mainCamera.forward / 3f) + new Vector3(0, -0.2f, 0), Quaternion.identity);
    }

    public void PasssthhroughToggle(bool turnOn)
    {
        if (passThroughIsOn != turnOn)
        {
            passThroughIsOn = turnOn;
            passThroughTransitionTime = time;
        }
    }

    private IEnumerator TransitionRoutine(float transitionTime)
    {
        yield return new WaitForSeconds(transitionTime);
        SpawnTransitionTriggers(transitionIndex);
    }
}
