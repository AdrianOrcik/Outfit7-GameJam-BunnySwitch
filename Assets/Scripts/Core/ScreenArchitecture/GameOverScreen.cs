using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : ScreenBehaviour
{
    public Button ResetBtn;

    public TMP_Text Score_Text;
    public TMP_Text BestScore_Text;

    void Start()
    {
        ResetBtn.onClick.AddListener(OnResetBtn);
    }

    private void OnEnable()
    {
        ScoreText(MainModel.GameManager.Score, Score_Text);
        ScoreText(MainModel.GameManager.BestScore, BestScore_Text);
    }

    public void ScoreText(int value, TMP_Text text)
    {
        StartCoroutine(ScoreCount(value, text));
    }

    private IEnumerator ScoreCount(int value, TMP_Text text)
    {
        int temp_value = 0;
        while (temp_value < value)
        {
            temp_value += 1;
            yield return new WaitForSeconds(0.0001f);
            text.text = temp_value.ToString();
        }
    }

    public void OnResetBtn()
    {
        MainModel.GameManager.OnRestartGame?.Invoke();
        MainModel.GameManager.LoadScene(1);
    }
}