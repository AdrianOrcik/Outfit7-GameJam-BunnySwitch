using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : ScreenBehaviour
{
    public Button StartGameBtn;

    private void Start()
    {
        StartGameBtn.onClick.AddListener(OnStartGame);
    }

    private void OnStartGame()
    {
        MainModel.GameManager.OnStartGame?.Invoke();
        ScreenManager.SetActiveScreen<StartScreen>(false);
    }
}