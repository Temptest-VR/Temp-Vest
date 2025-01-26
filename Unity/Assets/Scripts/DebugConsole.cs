using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    public static DebugConsole instance;
    [SerializeField] private TMP_Text consoleInput;

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

    public void Log(string text)
    {
        consoleInput.text = text + "\n";
    }
}
