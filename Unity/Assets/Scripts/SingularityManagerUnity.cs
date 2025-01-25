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
        singularityManager.DisconnectDevice();
    }
}
