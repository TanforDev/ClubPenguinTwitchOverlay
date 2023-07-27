using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventCaller : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEvent;

    public void CallOnEvent()
    {
        OnEvent?.Invoke();
    }
}
