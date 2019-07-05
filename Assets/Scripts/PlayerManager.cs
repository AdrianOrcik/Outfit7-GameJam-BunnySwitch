﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MainBehaviour
{
    public float playerSpeed = 5;
    public bool IsOnObstacle = false;
    public bool IsJumping = false;
    public bool IsOnPlatform = false;
    public float obstacleDistance = 0.1f;

    public void JumpUp()
    {
        IsOnObstacle = true;
        transform.position = new Vector2(transform.position.x, transform.position.y + 1);
    }

    public void JumpDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        IsOnObstacle = false;
    }

    public void MoveRight()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
        }
    }

    void Update()
    {
        MoveRight();

        RaycastHit2D hitForward = getRaycastForDiretion(Vector2.right, Constants.ObstacleLayer, obstacleDistance);
        if (hitForward && hitForward.transform)
        {
            Obstacle obstacle = hitForward.transform.GetComponent<Obstacle>();
            if (obstacle && obstacle.type == obstacleType.jump)
            {
                JumpUp();
            }

            if (obstacle && obstacle.type == obstacleType.kill)
            {
                MainModel.GameManager.OnGameOver?.Invoke();
                GameObject.Find("Player").GetComponent<Animator>().SetBool(Constants.PlayerDieObstacleAnimation, true);
                Debug.Log("Game Over");
            }

            if (obstacle && obstacle.type == obstacleType.trampoline)
            {
                Debug.Log("Big Jump");
                IsJumping = true;
                JumpUp();
                
                RaycastHit2D hitFloatingPLatform = getRaycastForDiretion(Vector2.right, Constants.TileLayer, 2.0f);
                if (hitFloatingPLatform && hitFloatingPLatform.transform)
                {
                    Debug.Log("Platform");
                    IsOnPlatform = true;
                    IsJumping = false;
                    JumpUp();
                }
            }
        }

        RaycastHit2D hitDown = getRaycastForDiretion(Vector2.down, Constants.TileLayer);
        if (hitDown && hitDown.transform)
        {
            Tile tile = hitDown.transform.GetComponent<Tile>();
            float heigh = Mathf.Floor(hitDown.distance);
            if (tile && (IsOnObstacle || IsJumping))
            {
                Debug.Log(heigh);
                for (int i = 0; i < heigh; i++)
                {
                    JumpDown();
                }
                IsJumping = false;
                IsOnPlatform = false;
            }
        }
        else if(MainModel.GameManager.IsPlaying && !IsOnPlatform)
        {
            Debug.Log("fall down");
            JumpDown();
            JumpDown();
            JumpDown();
            MainModel.GameManager.OnGameOver?.Invoke();
            GameObject.Find("Player").GetComponent<Animator>().SetBool(Constants.PlayerDieFallAnimation, true);
            CameraManager.IsTargeting = false;
        }
    }

    RaycastHit2D getRaycastForDiretion(Vector2 direction, string element, float distance = -1)
    {
        int mask = (LayerMask.GetMask(element));

        RaycastHit2D hit;
        Vector2 rayVector;
        Vector2 startPoint = transform.position;
        if (direction == Vector2.down)
        {
            startPoint.x = startPoint.x - 0.5f;
        }
        if (distance == -1)
        {
            rayVector = transform.TransformDirection(direction);
            hit = Physics2D.Raycast(startPoint, rayVector, mask);
        }
        else
        {
            rayVector = transform.TransformDirection(direction) * distance;
            hit = Physics2D.Raycast(startPoint, rayVector, distance, mask);
        }

        Debug.DrawRay(startPoint, rayVector, Color.red);
        return hit;
    }
}