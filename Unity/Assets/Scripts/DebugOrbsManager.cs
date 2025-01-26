using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DebugOrbsManager : MonoBehaviour
{
    public bool isDebugging = false;
    [SerializeField] private Transform OrbControllerHot;
    [SerializeField] private Transform OrbControllerCold;
    [SerializeField] private float thresdhold = 0.3f;
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;
    private int leftHandValue = 0;
    private int rightHandValue = 0;
    // private GrabInteractable hotGrabInteractable;
    // private GrabInteractable coldGrabInteractable;

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

        isDebugging = transform.GetChild(0).gameObject.activeSelf;
    }

    public void StartDebugging()
    {
        isDebugging = true;
        OrbControllerHot.gameObject.SetActive(true);
        OrbControllerCold.gameObject.SetActive(true);
    }

    public void StopDebugging()
    {
        isDebugging = false;
        OrbControllerHot.gameObject.SetActive(false);
        OrbControllerCold.gameObject.SetActive(false);
    }

    private void UpdateTemperatureValue()
    {
        // I need to make this update by distance for immersiveness
        /*foreach (var interactor in hotGrabInteractable.Interactors)
        {
            if (interactor.transform.parent.parent.GetComponent<Controller>().Handedness == Handedness.Left)
            {
                /*if (leftHandValue < 100)
                {
                    leftHandValue++;
                }#1#
                leftHandValue = 100;
            }
            else if (interactor.transform.parent.parent.GetComponent<Controller>().Handedness == Handedness.Right)
            {
                /*if (rightHandValue < 100)
                {
                    rightHandValue++;
                }#1#
                rightHandValue = 100;
            }
        }
        foreach (var interactor in coldGrabInteractable.Interactors)
        {
            if (interactor.transform.parent.parent.GetComponent<Controller>().Handedness == Handedness.Left)
            {
                /*if (leftHandValue > -100)
                {
                    leftHandValue--;
                }#1#
                this.leftHandValue = -100;
            }
            else if (interactor.transform.parent.parent.GetComponent<Controller>().Handedness == Handedness.Right)
            {
                /*if (rightHandValue > -100)
                {
                    rightHandValue--;
                }#1#
                rightHandValue = -100;
            }
        }*/
        
        int leftHandValueHot = 0;
        int leftHandValueCold = 0;
        int rightHandValueHot = 0;
        int rightHandValueCold = 0;
        
        /*int leftHandValueHot = (int) Vector3.Distance(OrbControllerHot.position, leftController.position);
        int leftHandValueCold = (int) Vector3.Distance(OrbControllerCold.position, leftController.position);
        int rightHandValueHot = (int) Vector3.Distance(OrbControllerHot.position, rightController.position);
        int rightHandValueCold = (int) Vector3.Distance(OrbControllerCold.position, rightController.position);*/

        if (Vector3.Distance(OrbControllerHot.position, leftController.position) < thresdhold)
        {
            leftHandValueHot = (int) ((thresdhold - Vector3.Distance(OrbControllerHot.position, leftController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(OrbControllerCold.position, leftController.position) < thresdhold)
        {
            leftHandValueCold = (int) ((thresdhold - Vector3.Distance(OrbControllerCold.position, leftController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(OrbControllerHot.position, rightController.position) < thresdhold)
        {
            rightHandValueHot = (int) ((thresdhold - Vector3.Distance(OrbControllerHot.position, rightController.position)) / thresdhold * 200);
        }
        if (Vector3.Distance(OrbControllerCold.position, rightController.position) < thresdhold)
        {
            rightHandValueCold = (int) ((thresdhold - Vector3.Distance(OrbControllerCold.position, rightController.position)) / thresdhold * 200);
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
