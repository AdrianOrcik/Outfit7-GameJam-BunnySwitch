using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MainBehaviour
{
    public Action OnGameOver;
    public Action OnStartGame;
    public bool IsPlaying { get; set; }

    private void Awake()
    {
        MainModel.GameManager = this;
    }

    private void Start()
    {
        OnGameOver += GameOver;
        OnStartGame += StartGame;
    }

    private void GameOver()
    {
        IsPlaying = false;
        ScreenManager.GetScreen<GameOverScreen>().gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<Animator>().SetBool(Constants.PlayerRunAnimation, false);
    }

    private void StartGame()
    {
        IsPlaying = true;
        GameObject.Find("Player").GetComponent<Animator>().SetBool(Constants.PlayerRunAnimation, true);
    }

    //TODO: Refactor for loadingManager
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}