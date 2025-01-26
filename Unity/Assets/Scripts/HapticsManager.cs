using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using Unity.VisualScripting;
using UnityEngine;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager instance;
    public HapticSource hapticSource;
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
        hapticSource.priority = 0;
        hapticSource.loop = true;
    }

    public void LeftAcceleratorEngaged()
    {
        hapticSource.Play(Controller.Left);
    }

    public void RightAcceleratorEngaged()
    {
        hapticSource.Play(Controller.Right);
    }

    public void AcceletorDisengaged()
    {
        hapticSource.Stop();
    }
    
    public void AcceleratorPositionChanged(float acceleratorPosition)
    {
        if (0.0f > acceleratorPosition || acceleratorPosition > 1.0f)
        {
            return;
        }

        // amplitude is in a range of 0.0f to 1.0f
        hapticSource.amplitude = acceleratorPosition;
        // frequencyShift is in a range of -1.0f to 1.0f
        hapticSource.frequencyShift = (acceleratorPosition * 2.0f) - 1.0f;
    }
}
