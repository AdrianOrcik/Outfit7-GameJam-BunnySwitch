using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Camera lerp curve tween
public class CameraManager : MainBehaviour
{
    public bool IsTargeting { get; set; }

    public Transform Target;
    private float DampTime = 7f;
    public Vector3 CameraOffset;

    private float margin = 0.1f;

    private void Start()
    {
        IsTargeting = true;
    }

//    void Update()
//    {
//        if (Target && IsTargeting)
//        {
//            float targetX = Target.position.x + CameraOffset.x;
//            float targetY = Target.position.y + CameraOffset.y;
//
//            if (Mathf.Abs(transform.position.x - targetX) > margin)
//                targetX = Mathf.Lerp(transform.position.x, targetX, 1 / DampTime * Time.deltaTime);
//
//            if (Mathf.Abs(transform.position.y - targetY) > margin)
//                targetY = Mathf.Lerp(transform.position.y, targetY, DampTime * Time.deltaTime);
//
//            transform.position = new Vector3(targetX, targetY, transform.position.z);
//        }
//    }

    public float FollowSpeed = 2f;

    private void Update()
    {
        if (Target && IsTargeting)
        {
            Vector3 newPosition = Target.position;
            newPosition.z = -10;
            transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
        }
    }
}