using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float playerSpeed = 5;
    public bool IsOnObstacle = false;
    public bool IsJumping = false;

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
        transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);

        RaycastHit2D hitForward = getRaycastForDiretion(Vector2.right, Constants.ObstacleLayer, 0.8f);
        if (hitForward && hitForward.transform)
        {
            Obstacle obstacle = hitForward.transform.GetComponent<Obstacle>();
            if (obstacle && obstacle.type == Obstacle.obstacleType.jump)
            {
                MoveUp();
            }
            if (obstacle && obstacle.type == Obstacle.obstacleType.kill)
            {
                Debug.Log("Game Over");
            }
            if (obstacle && obstacle.type == Obstacle.obstacleType.trampoline)
            {
                Debug.Log("Big Jump");
                IsJumping = true;
                MoveUp();
            }
        }
        
        RaycastHit2D hitDown = getRaycastForDiretion(Vector2.down, Constants.TileLayer);
        if (hitDown && hitDown.transform)
        {
            Tile tile = hitDown.transform.GetComponent<Tile>();
            if (tile && (IsOnObstacle || IsJumping))
            {
                MoveDown();
                IsJumping = false;
            }
        }

        RaycastHit2D hitDown_empty = getRaycastForDiretion(Vector2.down, Constants.EmptyTileLayer);
        if (hitDown_empty && hitDown_empty.transform && !IsJumping)
        {
            EmptyTile emptyTile = hitDown_empty.transform.GetComponent<EmptyTile>();
            if (emptyTile)
            {
                MoveDown();
                MoveDown();
                MoveDown();
            }
        }
    }

    RaycastHit2D getRaycastForDiretion(Vector2 direction, string element, float distance = -1)
    {
        
        int mask = (LayerMask.GetMask(element));

        RaycastHit2D hit;
        Vector2 rayVector;
        if (distance == -1)
        {
            rayVector = transform.TransformDirection(direction);
            hit = Physics2D.Raycast(transform.position, rayVector, mask);
        }
        else
        {
            rayVector = transform.TransformDirection(direction) * distance;
            hit = Physics2D.Raycast(transform.position, rayVector, distance, mask);
        }
        Debug.DrawRay(transform.position, rayVector, Color.red);
        return hit;
    }
}