using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    RaycastHit hit;

    void Update()
    {
        Vector2 right = transform.TransformDirection(Vector2.right) * 0.8f;
        Debug.DrawRay (transform.position, right, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right);
        Debug.Log(hit.transform.position);
        if (hit != null)
        {
            Obstacle jump = hit.collider.transform.GetComponent<Obstacle>();
            if (jump != null)
            {
                Debug.Log("jump over obstacle");
            }
//            float distance = Vector2.Distance(transform.position, hit.collider.transform.position);
//            Debug.Log(hit.point);
//            Debug.Log(hit.collider.transform.position);
//            if (hit.collider && distance <= 0.8f)
//            {
//                Debug.Log("hit collider");
//            }
        }
    }
}
