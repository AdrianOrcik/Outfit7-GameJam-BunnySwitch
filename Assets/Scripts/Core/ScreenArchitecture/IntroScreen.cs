using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : ScreenBehaviour
{
    public Button StartGameBtn;
    public Button CerditsBtn;
    public Button ExitBtn;

    private void Start()
    {
        StartGameBtn.onClick.AddListener(OnStartGame);
//        CerditsBtn.onClick.AddListener(openCredits);
//        ExitBtn.onClick.AddListener(exitGame);
    }

    private void OnStartGame()
    {
//            MainModel.GameManager.OnStartGame?.Invoke();
//            ScreenManager.SetActiveScreen<IntroScreen>(false);

        Debug.Log("Load Game");
        MainModel.GameManager.LoadScene(1);
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