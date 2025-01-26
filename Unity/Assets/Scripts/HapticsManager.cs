using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager instance;
    public HapticSource hapticSourceLeft;
    public HapticSource hapticSourceRight;
    public bool leftPlaying = false;
    public bool rightPlaying = false;
    
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
        hapticSourceLeft.priority = 0;
        hapticSourceLeft.loop = true;
        hapticSourceRight.priority = 0;
        hapticSourceRight.loop = true;
    }

    public void LeftAcceleratcorEngaged()
    {
        if (!leftPlaying)
        {
            hapticSourceLeft.Play();
            leftPlaying = true;
        }
    }

    public void RightAcceleratorEngaged()
    {
        if (!rightPlaying)
        {
            hapticSourceRight.Play();
            rightPlaying = true;
        }
    }

    public void LeftAcceletorDisengaged()
    {
        leftPlaying = false;
        hapticSourceLeft.Stop();
    }
    public void RightAcceletorDisengaged()
    {
        rightPlaying = false;
        hapticSourceRight.Stop();
    }
}
