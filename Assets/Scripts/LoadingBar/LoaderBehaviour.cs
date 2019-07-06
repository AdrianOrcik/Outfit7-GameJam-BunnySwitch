using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoaderBehaviour : MainBehaviour
{
    public Image FillImage;
    public TMP_Text PercentageText;
    float amount = 1f;

    private void Start()
    {
        OnStartLoadMainScene();
    }

    void OnStartLoadMainScene()
    {
        FillImage.fillAmount = 0f;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(FillImage.DOFillAmount(amount, 1).OnUpdate(UpdateText).SetEase(Ease.InExpo));
        mySequence.AppendInterval(0.5F).OnComplete(LoadLevel);
    }

    void UpdateText()
    {
        PercentageText.text = Mathf.RoundToInt(FillImage.fillAmount * 100) + "%";
    }

    void LoadLevel()
    {
        GameManager.LoadScene(1);
    }
}