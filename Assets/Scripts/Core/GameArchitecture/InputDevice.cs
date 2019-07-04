using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDevice : InputBase
{
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
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                firstPosition = touch.position;

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
            }
            else
            {
                Debug.Log("Tap");
            }

            OnResetPosition();
        }
    }
}