using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    public PlayerManager playerManager;
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("smallObstacle"))
        {
            Debug.Log("jump");
            playerManager.JumpUp();
        }
    }
    
//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("smallObstacle"))
//        {
//            playerManager.JumpDown();
//        }
//    }
}
