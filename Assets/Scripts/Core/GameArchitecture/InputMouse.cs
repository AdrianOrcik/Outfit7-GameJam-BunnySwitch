using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMouse : InputBase
{
    protected override void SwipeDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = firstPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastPosition = Input.mousePosition;

            if (Mathf.Abs(lastPosition.y - firstPosition.y) > dragDistance)
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
            }
            else
            {
                Debug.Log("Tap");
            }

            OnResetPosition();
        }
    }
}