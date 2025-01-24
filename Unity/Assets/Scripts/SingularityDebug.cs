using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingularityDebug : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    [SerializeField] private TMP_Text debugUI;

    public void DebugUI(string text)
    {
        debugUI.text = text;
    }
}

