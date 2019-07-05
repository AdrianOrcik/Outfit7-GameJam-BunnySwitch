using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Camera lerp curve tween
public class CameraManager : MainBehaviour
{
    public bool IsTargeting { get; set; }
    public float FollowSpeed = 2f;
    public Transform Target;
    public Vector3 CameraOffset;

    private void Start()
    {
        IsTargeting = true;
    }

    private void Update()
    {
        if (Target && IsTargeting)
        {
            Vector3 newPosition = Target.position;
            newPosition.z = -10;
            transform.position =
                Vector3.Slerp(transform.position, newPosition + CameraOffset, FollowSpeed * Time.deltaTime);
        }
    }
}