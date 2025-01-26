using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FireDoorsManager : MonoBehaviour
{
    public bool isDebugging = false;
    // [SerializeField] private Transform ControllerFire;
    // [SerializeField] private Transform ControllerSnow;
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorCenter;
    [SerializeField] private Transform doorRight;
    
    [SerializeField] private float thresdhold = 0.2f;
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;
    private int leftHandValue = 0;
    private int rightHandValue = 0;

    void Update()
    {
        if (isDebugging)
        {
            UpdateTemperatureValue();
            string leftOrbText = leftHandValue.ToString();
            string rightOrbText = rightHandValue.ToString();
            Debug.Log("left value: " + leftOrbText + "right value: " + rightOrbText);
            DebugConsole.instance.Log("left value: " + leftOrbText + ", right value: " + rightOrbText);
            SingularityManagerUnity.instance.UpdateBothHandValue(leftHandValue, rightHandValue);
        }

        isDebugging = transform.gameObject.activeSelf;
    }

    public void StartDebugging()
    {
        isDebugging = true;
        doorLeft.gameObject.SetActive(true);
        doorCenter.gameObject.SetActive(true);
        doorRight.gameObject.SetActive(true);
    }

    public void StopDebugging()
    {
        isDebugging = false;
        doorLeft.gameObject.SetActive(false);
        doorCenter.gameObject.SetActive(false);
        doorRight.gameObject.SetActive(false);
    }

    private void UpdateTemperatureValue()
    {
        int leftHandValueHot = 0;
        int leftHandValueCold = 0;
        int rightHandValueHot = 0;
        int rightHandValueCold = 0;

        if (Vector3.Distance(doorLeft.position, leftController.position) < thresdhold)
        {
            leftHandValueHot = (int) ((thresdhold - Vector3.Distance(doorLeft.position, leftController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(doorCenter.position, leftController.position) < thresdhold)
        {
            leftHandValueCold = (int) ((thresdhold - Vector3.Distance(doorCenter.position, leftController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(doorRight.position, leftController.position) < thresdhold)
        {
            leftHandValueCold = (int) ((thresdhold - Vector3.Distance(doorRight.position, leftController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(doorLeft.position, rightController.position) < thresdhold)
        {
            rightHandValueHot = (int) ((thresdhold - Vector3.Distance(doorLeft.position, rightController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(doorCenter.position, rightController.position) < thresdhold)
        {
            rightHandValueCold = (int) ((thresdhold - Vector3.Distance(doorCenter.position, rightController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(doorRight.position, rightController.position) < thresdhold)
        {
            rightHandValueCold = (int) ((thresdhold - Vector3.Distance(doorRight.position, rightController.position)) / thresdhold * 200);
        }
        
        leftHandValue = leftHandValueHot - leftHandValueCold;
        rightHandValue = rightHandValueHot - rightHandValueCold;
        
        if (leftHandValue > 100)
        {
            leftHandValue = 100;
        }
        else if (leftHandValue < -100)
        {
            leftHandValue = -100;
        }

        if (rightHandValue > 100)
        {
            rightHandValue = 100;
        }
        else if (rightHandValue < -100)
        {
            rightHandValue = -100;
        }
    }
}
