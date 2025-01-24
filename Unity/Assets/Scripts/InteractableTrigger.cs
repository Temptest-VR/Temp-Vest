using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    public void Selected(string input)
    {
        SingularityManagerUnity.instance.SendMessage(input);
    }
}
