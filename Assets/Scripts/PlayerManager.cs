using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MainBehaviour
{
    public float playerSpeed = 5;
    public bool IsOnObstacle = false;
    public bool IsJumping = false;
    public float obstacleDistance = 0.1f;

    private void Start()
    {
        InputManager.instance.OnSwipe += OnPlayerBounce;
    }

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

    public void OnPlayerBounce()
    {
        StartCoroutine(PlayerBounceRoutine());
    }

    private IEnumerator PlayerBounceRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        Vector3 bounceUp = new Vector3(0f, 0.2f, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(transform.position.y + bounceUp.y, 0.15f)
            .SetEase(Ease.OutBack));
        mySequence.Append(transform.DOMoveY(transform.position.y, 0.2f)
            .SetEase(Ease.InExpo));
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
            }
        }

        RaycastHit2D hitDown = getRaycastForDiretion(Vector2.down, Constants.TileLayer);
        if (hitDown && hitDown.transform)
        {
            Tile tile = hitDown.transform.GetComponent<Tile>();
            if (tile && (IsOnObstacle || IsJumping))
            {
                JumpDown();
                IsJumping = false;
            }
        }

        RaycastHit2D hitDown_empty = getRaycastForDiretion(Vector2.down, Constants.EmptyTileLayer);
        if (hitDown_empty && hitDown_empty.transform && !IsJumping)
        {
            EmptyTile emptyTile = hitDown_empty.transform.GetComponent<EmptyTile>();
            if (emptyTile)
            {
                //TODO: generic 

                CameraManager.IsTargeting = false;
                JumpDown();
                JumpDown();
                JumpDown();
                MainModel.GameManager.OnGameOver?.Invoke();
                GameObject.Find("Player").GetComponent<Animator>().SetBool(Constants.PlayerDieFallAnimation, true);
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