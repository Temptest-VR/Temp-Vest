using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    public void Selected(string input)
    {
        if (SingularityManagerUnity.instance == null)
        {
            SingularityManagerUnity.instance = FindObjectOfType<SingularityManagerUnity>();
        }
        SingularityManagerUnity.instance.singularityManager.sendMessage(input);
    }
}
