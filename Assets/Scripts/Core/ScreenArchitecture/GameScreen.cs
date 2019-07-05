using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScreen : ScreenBehaviour
{
    public GameObject ReadyPanel;

    public TMP_Text GetReadyText;
    public TMP_Text LevelScore;

    private void Start()
    {
        StartCoroutine(StartRoutine());
    }

    public void IncrementScore(int value)
    {
        LevelScore.text = value.ToString();
    }

    private IEnumerator StartRoutine()
    {
        GetReadyText.text = string.Format("3");
        yield return new WaitForSeconds(Constants.GET_READY_TIME);
        GetReadyText.text = string.Format("2");
        yield return new WaitForSeconds(Constants.GET_READY_TIME);
        GetReadyText.text = string.Format("1");
        yield return new WaitForSeconds(Constants.GET_READY_TIME);
        GetReadyText.text = string.Format("GO");
        MainModel.GameManager.OnStartGame?.Invoke();
        yield return new WaitForSeconds(Constants.FADE_OUT_READY_SCREEN_TIME);
        ReadyPanel.gameObject.SetActive(false);
    }
}