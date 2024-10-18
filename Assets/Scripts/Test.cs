using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public UnityEvent OnHurt;
    void Start()
    {

    }
    void Update()
    {
        //if (Gamepad.current == null)
        if (Keyboard.current == null)
        {
            return;
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log("lKey");
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("kKey");
        }
    }
}
