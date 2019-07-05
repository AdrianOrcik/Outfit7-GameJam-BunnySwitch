using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : ScreenBehaviour
{
    public Button StartGameBtn;
    public Button CerditsBtn;
    public Button ExitBtn;

    private void Start()
    {
        StartGameBtn.onClick.AddListener(OnStartGame);
        CerditsBtn.onClick.AddListener(openCredits);
        ExitBtn.onClick.AddListener(exitGame);
    }

    private void OnStartGame()
    {
        MainModel.GameManager.OnStartGame?.Invoke();
        ScreenManager.SetActiveScreen<StartScreen>(false);
    }

    private void openCredits()
    {
        ScreenManager.GetScreen<CreditsScreen>().gameObject.SetActive(true);
    }
    
    private void exitGame()
    {
        Debug.Log("Exit");
    }
}