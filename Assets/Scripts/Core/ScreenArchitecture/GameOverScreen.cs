using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : ScreenBehaviour
{
    public Button ResetBtn;

    void Start()
    {
        ResetBtn.onClick.AddListener(OnResetBtn);
    }

    public void OnResetBtn()
    {
        MainModel.GameManager.LoadScene(0);
    }
}