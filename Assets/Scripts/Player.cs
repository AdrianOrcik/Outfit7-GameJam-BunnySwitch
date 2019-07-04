using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
//    private PlayerManager playerManager;
//    RaycastHit hit;
//    
//    void Start()
//    {
//        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
//    }
//
//    void Update()
//    {
//        Vector2 right = transform.TransformDirection(Vector2.right) * 0.8f;
//        Debug.DrawRay (transform.position, right, Color.red);
//
//        int obstacle_mask = (LayerMask.GetMask("Obstacle"));
//        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, Vector2.right, 0.8f, obstacle_mask);
//        if (hitForward && hitForward.transform)
//        {
//            Obstacle obstacle = hitForward.transform.GetComponent<Obstacle>();
//            if (obstacle)
//            {
//                playerManager.JumpUp();
//            }
//
//        }
//        
//        Vector2 down = transform.TransformDirection(Vector2.down) * 0.8f;
//        Debug.DrawRay (transform.position, down, Color.red);
//        
//        int tile_mask = (LayerMask.GetMask("Tile"));
//        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, tile_mask);
//        if (hitDown && hitDown.transform)
//        {
////            Tile obstacle = hitForward.transform.GetComponent<Tile>();
////            if (obstacle)
////            {
////                playerManager.JumpUp();
////            }
//
//        }
//    }
}
