using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputBase
{
    public Action OnSwipeUp;
    public Action OnSwipeDown;
    public Action OnSwipe;

    protected Vector3 firstPosition;
    protected Vector3 lastPosition;
    protected float dragDistance;

    protected abstract void SwipeDetection();

    protected void OnResetPosition()
    {
        firstPosition = Vector3.zero;
        lastPosition = Vector3.zero;
    }

    public void Update()
    {
        SwipeDetection();
    }

    public void Init()
    {
        // ReSharper disable once PossibleLossOfFraction
        dragDistance = Screen.height * 15 / 100;
    }
}