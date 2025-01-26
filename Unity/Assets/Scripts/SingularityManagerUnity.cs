using System;
using System.Collections;
using System.Collections.Generic;
using Sngty;
using TMPro;
using UnityEngine;

public class SingularityManagerUnity : MonoBehaviour
{
    public static SingularityManagerUnity instance;
    public SingularityManager singularityManager;
    [SerializeField] private SingularityDebug singularityDebug;
    [SerializeField] private float updateSingularityTimer = 0.2f;
    [SerializeField] private HapticsManager hapticsManager;
    private float lastTimeSinceSingularityUpdate = 0f;
    private float time  = 0.0f;
    private int leftHandValue = 0;
    private int rightHandValue = 0;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > lastTimeSinceSingularityUpdate + updateSingularityTimer)
        {
            lastTimeSinceSingularityUpdate = time;
            SendSingularityMessage();
        }
        // Debug.Log("leftHandValue: " + leftHandValue + " rightHandValue: " + rightHandValue);
        if (leftHandValue > 0)
        {
            hapticsManager.LeftAcceleratcorEngaged();
        }
        else
        {
            hapticsManager.LeftAcceletorDisengaged();
        }
        if (rightHandValue > 0)
        {
            hapticsManager.RightAcceleratorEngaged();
        }
        else
        {
            hapticsManager.RightAcceletorDisengaged();
        }
    }

    public void OnConnected()
    {
        Debug.Log($"Connected");
    }

    public void OnMessageReceived(string message)
    {
        Debug.Log($"Received {message}");
        if (singularityDebug != null)
        {
            singularityDebug.DebugUI(message);
        }
    }

    private void OnDestroy()
    {
        singularityManager.DisconnectAll();
    }

    public void UpdateLeftHandValue(int value)
    {
        leftHandValue = value;
    }

    public void UpdateRightHandValue(int value)
    {
        rightHandValue = value;
    }

    public void UpdateBothHandValue(int leftValue, int rightValue)
    {
        leftHandValue = leftValue;
        rightHandValue = rightValue;
    }

    private void SendSingularityMessage()
    {
        if (leftHandValue > 100)
        {
            leftHandValue = 100;
        }
        if (leftHandValue < -100)
        {
            leftHandValue = -100;
        }
        if (rightHandValue > 100)
        {
            rightHandValue = 100;
        }
        if (rightHandValue < -100)
        {
            rightHandValue = -100;
        }
        singularityManager.sendMessage(leftHandValue + " " + rightHandValue);
    }
}
