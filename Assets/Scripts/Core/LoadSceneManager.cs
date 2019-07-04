using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AsyncOperation = UnityEngine.AsyncOperation;
using UnityEngine.UI;
using System;
using TMPro;


public class LoadSceneManager : MonoBehaviour
{
    public void LoadSceneAdditive(int sceneIndex, Image loadingBar, float initTime, TMP_Text percentage)
    {
        StartCoroutine(LoadAsynchrounsly(sceneIndex, loadingBar, initTime, percentage));
    }

    IEnumerator LoadAsynchrounsly(int sceneIndex, Image loadingBar, float initTime, TMP_Text percentage)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        float duration = 1f;
        float normalizedTime = initTime;
        while (normalizedTime <= 1f || !operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            loadingBar.fillAmount = normalizedTime;
            percentage.text = Mathf.RoundToInt(loadingBar.fillAmount * 100) + "%";
            normalizedTime += Time.deltaTime / progress + duration;
            yield return null;
        }
        
        SceneManager.UnloadSceneAsync(currentScene);
    }
}