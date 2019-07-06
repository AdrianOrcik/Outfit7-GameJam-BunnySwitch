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

//        bool platform = false;
//        int mask = (LayerMask.GetMask(Constants.TileLayer));
//        RaycastHit2D hitPlatform =  Physics2D.Raycast(CharacterTransform.position, new Vector2(3,2), mask);//= Physics2D.Raycast(CharacterTransform.position, new Vector2(3,2));
//        Debug.DrawRay(CharacterTransform.position, new Vector2(3,2));
//        if (hitPlatform.collider != null && hitPlatform.transform.GetComponent<Obstacle>())
//        {
//            platform = true;
//        }
//        
//        if (platform)
//        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOMoveY(defaultYpos + bounceUp.y, Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME)
                .SetEase(Ease.OutExpo));
            mySequence.Append(transform
                .DOMoveY(defaultYpos + finalBounceUp.y, Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_TIME)
                .SetEase(Ease.InExpo));
            mySequence.AppendInterval(Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME +
                                      Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_TIME)
                .OnComplete(OnJumpCompleted);

            Sequence animationSequence = DOTween.Sequence();
            animationSequence.AppendInterval(Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME)
                .OnComplete(EndJumpUpAnimation);
//        }
//        else
//        {
//            Sequence mySequence = DOTween.Sequence();
//            mySequence.Append(transform.DOMoveY(defaultYpos + bounceUp.y, Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME)
//                .SetEase(Ease.OutExpo));
//            mySequence.Append(transform
//                .DOMoveY(defaultYpos + defaultYpos, Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME)
//                .SetEase(Ease.InExpo));
//            mySequence.AppendInterval(Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME +
//                                      Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_LONG)
//                .OnComplete(OnJumpCompleted);
//            
//            Sequence animationSequence = DOTween.Sequence();
//            animationSequence.AppendInterval(Constants.PLAYER_TRAMPOLINE_JUMP_UP_TIME + Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_LONG)
//                .OnComplete(EndJumpUpAnimation);
//            
//        }

        Animator.SetBool(Constants.PlayerJumpUp, true);
        IsJumping = true;
    }

    void OnJumpCompleted()
    {
        IsJumping = false;
    }

    void EndJumpUpAnimation()
    {
        IsJumping = false;
        Animator.SetBool(Constants.PlayerJumpUp, false);
    }

    void EndJumpDownAnimation()
    {
        Animator.SetBool(Constants.PlayerJumpDown, false);
    }

    public void JumpDown(float distance)
    {
        if (CurrentTile != null && !IsJumping)
        {
            if (distance > jumpDownDistance && CurrentTile.transform.position.y >= platformPosition)
            {
                Animator.SetBool(Constants.PlayerJumpDown, true);
                Sequence animationSequence = DOTween.Sequence();
                animationSequence.AppendInterval(0.2f)
                    .OnComplete(EndJumpDownAnimation);
            }

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform
                .DOMoveY(CurrentTile.position_Y, Constants.PLAYER_TRAMPOLINE_JUMP_DOWN_TIME)
                .SetEase(Ease.OutExpo));
        }
    }

    public IEnumerator FallDown()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform
            .DOMoveX(transform.position.x + 0.1f, 0.5f)
            .SetEase(Ease.OutExpo));

        yield return new WaitForSeconds(0.5f);

        mySequence.Append(transform
            .DOMoveY(transform.position.y - 2, 1f)
            .SetEase(Ease.OutExpo));
    }

    public void MoveRight()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
        }
    }

    private float interectableDistance = 0.5f;
    private float EmptyTileDistance = 1.2f;
    private float tileDistance = 1.5f;
    private float emptyTileDistance = 1.5f;
    private float jumpDownDistance = 1.5f;
    private int platformPosition = 0;
    private float platformDistance = 3.0f;

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
                        interactable.gameObject.transform.GetChild(0).GetComponent<Animator>()
                            .SetBool(Constants.MushroomBounce, true);
                        JumpUp();
                        break;
                    case ObstacleType.kill:
                        MainModel.GameManager.OnGameOver?.Invoke();
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

            float distance = Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position);
            if (Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position) >
                tileDistance)
            {
                JumpDown(Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position));
            }
        }

        RaycastHit2D hitEmpty = Physics2D.Raycast(CharacterTransform.position, Vector2.down);
        Debug.DrawRay(CharacterTransform.position, Vector2.down, Color.red);
        if (hitEmpty.collider != null && hitEmpty.collider.GetComponent<EmptyTile>())
        {
            EmptyTile tile = hitEmpty.collider.GetComponent<EmptyTile>();
            if (Vector3.Distance(tile.gameObject.transform.position, CharacterTransform.position) <
                emptyTileDistance)
            {
                MainModel.GameManager.OnGameOver?.Invoke();
                StartCoroutine(FallDown());
                Animator.SetBool(Constants.PlayerDieFallAnimation, true);
            }
        }
    }
}