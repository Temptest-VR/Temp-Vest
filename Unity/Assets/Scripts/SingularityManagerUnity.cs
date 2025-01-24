using System;
using System.Collections;
using System.Collections.Generic;
using Sngty;
using UnityEngine;

public class SingularityManagerUnity : MonoBehaviour
{
    [SerializeField] private SingularityManager singularityManager;

    public void OnConnected()
    {
        Debug.Log($"Connected");
    }

    public void OnMessageReceived(string message)
    {
        Debug.Log($"Received {message}");
    }

    private void OnDestroy()
    {
        singularityManager.DisconnectDevice();
    }

    void Update()
    {
        singularityManager.sendMessage("0, 1, 2, 3, 4, 5");
    }
}
