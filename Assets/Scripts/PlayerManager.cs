using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //public Player player;
    public float playerSpeed = 5;
    public float jumpForce = 20;
    public bool IsOnObstacle = false;

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

    public void MoveRight()
    {
        //MOVING
        transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
    }

    void Update()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            MoveRight();
        }

        Vector2 right = transform.TransformDirection(Vector2.right) * 0.8f;
        Debug.DrawRay(transform.position, right, Color.red);

        int obstacle_mask = (LayerMask.GetMask("Obstacle"));
        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, Vector2.right, 0.8f, obstacle_mask);
        if (hitForward && hitForward.transform)
        {
            Obstacle obstacle = hitForward.transform.GetComponent<Obstacle>();
            if (obstacle)
            {
                MoveUp();
            }
        }

        Vector2 down = Vector2.down;
        Debug.DrawRay(transform.position, down, Color.red);

        int tile_mask = (LayerMask.GetMask("Tile"));
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, down, tile_mask);
        if (hitDown && hitDown.transform)
        {
            Tile tile = hitDown.transform.GetComponent<Tile>();
            if (tile && IsOnObstacle)
            {
                MoveDown();
            }
        }

        Vector2 down_empty = Vector2.down * 10;
        Debug.DrawRay(transform.position, down_empty, Color.red);

        int empty_tile_mask = (LayerMask.GetMask("EmptyTile"));
        RaycastHit2D hitDown_empty = Physics2D.Raycast(transform.position, down_empty, empty_tile_mask);
        if (hitDown_empty && hitDown_empty.transform)
        {
            EmptyTile emptyTile = hitDown_empty.transform.GetComponent<EmptyTile>();
            if (emptyTile)
            {
                MainModel.GameManager.OnGameOver?.Invoke();
                MoveDown();
                MoveDown();
                MoveDown();
            }
        }
    }
}