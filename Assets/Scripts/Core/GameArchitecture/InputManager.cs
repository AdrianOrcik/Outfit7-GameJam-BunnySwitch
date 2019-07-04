using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MainBehaviour
{
    public InputBase instance = null;

    InputManager()
    {
#if !UNITY_EDITOR && ( UNITY_ANDROID || UNITY_IOS )
        instance = new InputDevice();
#else
        instance = new InputMouse();
#endif
        instance.Init();
    }

    private void Update()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            instance?.Update();
        }
    }
}