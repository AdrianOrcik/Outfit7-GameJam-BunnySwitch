using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreen : ScreenBehaviour
{
    public Button ReturnBtn;

    void Start()
    {
        ReturnBtn.onClick.AddListener(OnReturnBtn);
    }

    public void OnReturnBtn()
    {
        ScreenManager.GetScreen<CreditsScreen>().gameObject.SetActive(false);
        
    }
}