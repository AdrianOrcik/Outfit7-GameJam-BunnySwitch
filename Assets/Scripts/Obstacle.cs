using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : Interactable
{
    public obstacleType type = obstacleType.none;

    private void Start()
    {
        InputManager.instance.OnSwipe += OnObstacleBounce;
    }

    public void OnObstacleBounce()
    {
        if (BoxCollider2D.enabled)
        {
            StartCoroutine(ObstacleBounceRoutine());
        }
    }

    private IEnumerator ObstacleBounceRoutine()
    {
        yield return new WaitForSeconds(Constants.PLAYER_BOUCE_WAIT_TIME);
        Vector3 bounceUp = new Vector3(0f, 0.1f, 0);
        float defaultYPos = transform.position.y;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(transform.position.y + bounceUp.y, Constants.PLAYER_BOUCE_UP_TIME)
            .SetEase(Ease.OutBack));
        mySequence.Append(transform.DOMoveY(defaultYPos, Constants.PLAYER_BOUCE_DOWN_TIME)
            .SetEase(Ease.InExpo));
    }
}