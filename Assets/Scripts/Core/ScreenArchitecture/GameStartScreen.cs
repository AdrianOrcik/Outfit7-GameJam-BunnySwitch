using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartScreen : ScreenBehaviour
{
    public Button PlayGameBtn;

    void Awake()
    {
        if (MainModel.GameManager.InitScreenSeen)
        {
            ScreenManager.GetScreen<GameStartScreen>().gameObject.SetActive(false);
            ScreenManager.GetScreen<GameScreen>().gameObject.SetActive(true);
        }
        else
        {
            MainModel.GameManager.InitScreenSeen = true;
        }
    }

    void Start()
    {
        PlayGameBtn.onClick.AddListener(OnPlayGame);
    }

    public void OnPlayGame()
    {
        ScreenManager.GetScreen<GameStartScreen>().gameObject.SetActive(false);
        ScreenManager.GetScreen<GameScreen>().gameObject.SetActive(true);
    }
}