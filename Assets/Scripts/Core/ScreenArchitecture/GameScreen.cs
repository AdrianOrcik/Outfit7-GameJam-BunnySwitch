using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScreen : ScreenBehaviour
{
    public TMP_Text GetReadyText;

    private void Start()
    {
        StartCoroutine(StartRoutine());
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
        ScreenManager.GetScreen<GameScreen>().gameObject.SetActive(false);
    }
}