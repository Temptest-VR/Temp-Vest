using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FireSnowManager : MonoBehaviour
{
    public bool isDebugging = false;
    [SerializeField] private Transform ControllerFire;
    [SerializeField] private Transform ControllerSnow;
    [SerializeField] private float thresdhold = 0.3f;
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
        ControllerFire.gameObject.SetActive(true);
        ControllerSnow.gameObject.SetActive(true);
    }

    public void StopDebugging()
    {
        isDebugging = false;
        ControllerFire.gameObject.SetActive(false);
        ControllerSnow.gameObject.SetActive(false);
    }

    private void UpdateTemperatureValue()
    {
        int leftHandValueHot = 0;
        int leftHandValueCold = 0;
        int rightHandValueHot = 0;
        int rightHandValueCold = 0;
        Vector3 controllerFireNormalized = new Vector3(ControllerFire.position.x, 0f, ControllerFire.position.z);
        Vector3 controllerIceNormalized = new Vector3(ControllerSnow.position.x, 0f, ControllerSnow.position.z);
        Vector3 leftHandNormalized = new Vector3(leftController.position.x, 0f, leftController.position.z);
        Vector3 rightHandNormalized = new Vector3(rightController.position.x, 0f, rightController.position.z);

        if (Vector3.Distance(controllerFireNormalized, leftHandNormalized) < thresdhold)
        {
            leftHandValueHot = (int) ((thresdhold - Vector3.Distance(controllerFireNormalized, leftHandNormalized)) / thresdhold * 200);
        }
        if (Vector3.Distance(controllerIceNormalized, leftHandNormalized) < thresdhold)
        {
            leftHandValueCold = (int) ((thresdhold - Vector3.Distance(controllerIceNormalized, leftHandNormalized)) / thresdhold * 200);
        }
        if (Vector3.Distance(controllerFireNormalized, rightHandNormalized) < thresdhold)
        {
            rightHandValueHot = (int) ((thresdhold - Vector3.Distance(controllerFireNormalized, rightHandNormalized)) / thresdhold * 200);
        }
        if (Vector3.Distance(controllerIceNormalized, rightHandNormalized) < thresdhold)
        {
            rightHandValueCold = (int) ((thresdhold - Vector3.Distance(controllerIceNormalized, rightHandNormalized)) / thresdhold * 200);
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
