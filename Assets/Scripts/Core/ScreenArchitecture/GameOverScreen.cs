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
        GameObject.Find("PlayScore").GetComponent<TMPro.TextMeshProUGUI>().text =
            MainModel.GameManager.Score.ToString();
    }

    public void OnResetBtn()
    {
        MainModel.GameManager.OnRestartGame?.Invoke();
        MainModel.GameManager.LoadScene(1);
    }
}