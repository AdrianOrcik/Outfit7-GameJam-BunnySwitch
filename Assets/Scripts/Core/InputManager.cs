using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MainBehaviour
{
    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private float dragDistance;

    public bool IsSwipe()
    {
        return IsSwipeUp() || IsSwipeDown();
    }

    public bool IsSwipeUp()
    {
        return lastPosition.y > firstPosition.y;
    }

    public bool IsSwipeDown()
    {
        return lastPosition.y < firstPosition.y;
    }

    private void Start()
    {
        // ReSharper disable once PossibleLossOfFraction
        dragDistance = Screen.height * 15 / 100;
    }

    private void Update()
    {
        SwipeDetection();
    }

    private void SwipeDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPosition = Input.mousePosition;
            lastPosition = firstPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastPosition = Input.mousePosition;

            if (Mathf.Abs(lastPosition.y - firstPosition.y) > dragDistance)
            {
                if (lastPosition.y > firstPosition.y)
                {
                    //Up swipe
                    Debug.Log("Up Swipe");
                }
                else
                {
                    //Down swipe
                    Debug.Log("Down Swipe");
                }
            }
            else
            {
                Debug.Log("Tap");
            }

            OnResetPosition();
        }
    }

    private void OnResetPosition()
    {
        firstPosition = Vector3.zero;
        lastPosition = Vector3.zero;
    }
}