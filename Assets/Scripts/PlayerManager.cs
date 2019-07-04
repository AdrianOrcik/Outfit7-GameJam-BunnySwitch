using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //public Player player;
    public float playerSpeed = 5;
    public float jumpForce = 20;
    public bool IsOnObstacle = false;

    void Start()
    {
//        player = Instantiate(player);
//        transform.position = new Vector2(0, 1);
    }


//    public void JumpUp()
//    {
//        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1);
//    }
//    
//    public void JumpDown()
//    {
//        player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y -1);
//    }

    public void MoveUp()
    {
        IsOnObstacle = true;
        transform.position = new Vector2(transform.position.x, transform.position.y + 1);
    }

    public void MoveDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        IsOnObstacle = false;
    }

    void Update()
    {
        //MOVING
        transform.Translate(Vector3.right * Time.deltaTime);


        Vector2 right = transform.TransformDirection(Vector2.right) * 0.8f;
        Debug.DrawRay(transform.position, right, Color.red);

        int obstacle_mask = (LayerMask.GetMask("Obstacle"));
        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, Vector2.right, 0.8f, obstacle_mask);
        if (hitForward && hitForward.transform)
        {
            Obstacle obstacle = hitForward.transform.GetComponent<Obstacle>();
            if (obstacle)
            {
                if (obstacle.IsInteractable)
                {
                    MoveUp();
                }
            }
        }

        Vector2 down = transform.TransformDirection(Vector2.down);
        Debug.DrawRay(transform.position, down, Color.red);

        int tile_mask = (LayerMask.GetMask("Tile"));
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, tile_mask);
        if (hitDown && hitDown.transform)
        {
            Tile tile = hitDown.transform.GetComponent<Tile>();
            if (tile && IsOnObstacle)
            {
                if (tile.IsInteractable)
                {
                    MoveDown();
                }
            }
        }
    }
}