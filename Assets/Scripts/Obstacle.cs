using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : Interactable
{
    private void OnEnable()
    {
        InputManager.instance.OnSwipe += OnObstacleBounce;
    }

    private void OnDisable()
    {
        // ReSharper disable once DelegateSubtraction
        if (InputManager.instance.OnSwipe != null) InputManager.instance.OnSwipe -= OnObstacleBounce;
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
        yield return new WaitForSeconds(Constants.OBSTACLE_BOUCE_WAIT_TIME);
        Vector3 bounceUp = new Vector3(0f, 0.2f, 0);
        float defaultYPos = transform.position.y;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(transform.position.y + bounceUp.y, Constants.OBSTACLE_BOUCE_UP_TIME)
            .SetEase(Ease.OutBack));
        mySequence.Append(transform.DOMoveY(defaultYPos, Constants.OBSTACLE_BOUCE_DOWN_TIME)
            .SetEase(Ease.InExpo));
    }

    public void Animate()
    {
        if (ObstacleType == ObstacleType.coin)
        {
            GameScreen gameScreen = ScreenManager.GetScreen<GameScreen>();
            transform.DOMove(gameScreen.LevelScore.transform.position, 1f).SetEase(Ease.Linear);
        }
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}