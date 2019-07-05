using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MainBehaviour
{
    public float playerSpeed = 5;
    public bool IsJumping;

    public Animator Animator;
    public Transform CharacterTransform;
    public Tile CurrentTile;

    private void Start()
    {
    }

    public void JumpUp()
    {
        Vector3 bounceUp = new Vector3(0f, 3f, 0);
        Vector3 finalBounceUp = new Vector3(0f, 2f, 0);
        float defaultYpos = transform.position.y;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(defaultYpos + bounceUp.y, Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME)
            .SetEase(Ease.OutExpo));
        mySequence.Append(transform.DOMoveY(defaultYpos + finalBounceUp.y, Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_TIME)
            .SetEase(Ease.InExpo));
        mySequence.AppendInterval(Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME + Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_TIME)
            .OnComplete(OnJumpCompleted);
        
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.AppendInterval(Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME)
            .OnComplete(EndJumpAnimation);
            
        Animator.SetBool(Constants.PlayerJumpUp, true);
        IsJumping = true;
    }

    void OnJumpCompleted()
    {
        IsJumping = false;
    }

    void EndJumpAnimation()
    {
        Animator.SetBool(Constants.PlayerJumpUp, false);
    }

    public void JumpDown()
    {
        if (CurrentTile != null && !IsJumping)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform
                .DOMoveY(CurrentTile.position_Y, Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_TIME)
                .SetEase(Ease.OutExpo));
        }
    }

    public void MoveRight()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
        }
    }

    private float interectableDistance = 0.5f;
    private float tileDistance = 1f;
    private float EmptyTileDistance = 1.5f;

    void Update()
    {
        MoveRight();

        RaycastHit2D hitInterectables = Physics2D.Raycast(CharacterTransform.position, Vector2.right);
        Debug.DrawRay(CharacterTransform.position, Vector2.right, Color.red);
        if (hitInterectables.collider != null && hitInterectables.collider.GetComponent<Interactable>())
        {
            Interactable interactable = hitInterectables.collider.GetComponent<Interactable>();
            if (Vector3.Distance(interactable.gameObject.transform.position, CharacterTransform.position) <
                interectableDistance)
            {
                switch (interactable.ObstacleType)
                {
                    case ObstacleType.trampoline:
                        interactable.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool(Constants.MushroomBounce, true);
                        JumpUp();
                        break;
                    case ObstacleType.kill:
                        Debug.Log("Kill");
                        MainModel.GameManager.IsPlaying = false;
                        Animator.SetBool(Constants.PlayerDieObstacleAnimation, true);
                        break;
                    case ObstacleType.jump:
                        Animator.SetBool(Constants.PlayerDieFallAnimation, true);
                        break;
                }
            }
        }

        RaycastHit2D hitGround = Physics2D.Raycast(CharacterTransform.position, Vector2.down);
        Debug.DrawRay(CharacterTransform.position, Vector2.down, Color.red);
        if (hitGround.collider != null && hitGround.collider.GetComponent<Tile>())
        {
            Tile tile = hitGround.collider.GetComponent<Tile>();
            CurrentTile = tile;
            if (Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position) >
                tileDistance)
            {
                JumpDown();
            }
        }
        
        RaycastHit2D hitEmpty = Physics2D.Raycast(CharacterTransform.position, Vector2.down);
        Debug.DrawRay(CharacterTransform.position, Vector2.down, Color.red);
        if (hitEmpty.collider != null && hitEmpty.collider.GetComponent<EmptyTile>())
        {
            //Debug.Log("fall test");
            EmptyTile tile = hitEmpty.collider.GetComponent<EmptyTile>();
            //Debug.Log(Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position));
            if (Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position) <
                EmptyTileDistance)
            {
                Animator.SetBool(Constants.PlayerDieFallAnimation, true);
               Debug.Log("fall");
            }
        }
    }
}