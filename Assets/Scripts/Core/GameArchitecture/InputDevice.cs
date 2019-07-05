﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDevice : InputBase
{
    private bool isTouchMoved = false;

    protected override void SwipeDetection()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstPosition = touch.position;
                lastPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lastPosition = touch.position;
                isTouchMoved = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                firstPosition = touch.position;

                if (Mathf.Abs(lastPosition.y - firstPosition.y) > dragDistance)
                {
                    if (isTouchMoved)
                    {
                        if (lastPosition.y > firstPosition.y)
                        {
                            OnSwipeUp?.Invoke();
                        }
                        else
                        {
                            OnSwipeDown?.Invoke();
                        }

                        OnSwipe?.Invoke();
                        isTouchMoved = false;
                    }
                }
            }
            else
            {
                Debug.Log("Tap");
            }

            OnResetPosition();
        }
    }
}