using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MainBehaviour
{
    public Action OnGameOver;
    public Action OnStartGame;
    public Action OnRestartGame;

    public bool IsPlaying { get; set; }
    public PlayerManager PlayerManager { get; set; }
    public int Score { get; set; }
    private GameScreen GameScreen;

    public bool InitScreenSeen { get; set; }
    private bool IsGameOver = false;

    private void Awake()
    {
        MainModel.GameManager = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        OnGameOver += GameOver;
        OnStartGame += StartGame;
        OnRestartGame += RestartGame;

        ResourceManager.LoadResources();
    }

    private void GameOver()
    {
        if (!IsGameOver)
        {
            StartCoroutine(GameOverRoutine());
            IsGameOver = true;
        }
    }

    private IEnumerator GameOverRoutine()
    {
        Debug.Log("GameOver");
        IsPlaying = false;
        CameraManager.IsTargeting = false;

        //FindObjectOfType<PlayerManager>().Animator.SetBool(Constants.PlayerRunAnimation, false);

        yield return new WaitForSeconds(1f);
        ScreenManager.GetScreen<GameOverScreen>().gameObject.SetActive(true);
        ScreenManager.GetScreen<GameScreen>().gameObject.SetActive(false);
        MainModel.ResourceManager.DisablePool();
    }

    private void StartGame()
    {
        IsPlaying = true;
        PlayerManager = FindObjectOfType<PlayerManager>();
        PlayerManager.Animator.SetBool(Constants.PlayerRunAnimation, true);
        GameScreen = ScreenManager.GetScreen<GameScreen>();

        StartCoroutine(IncreasingScore());
    }

    private void RestartGame()
    {
        IsGameOver = false;
        LayerManager.LayerBlocks = new List<LayerBlock>();
        ScreenManager.GetScreen<GameOverScreen>().gameObject.SetActive(false);
        Score = 0;
    }

    //Use for bonus increase of score
    public void IncreaseScore(int value)
    {
        Score += value;
        GameScreen.IncrementScore(Score);
    }

    //use for distance score increase
    private IEnumerator IncreasingScore()
    {
        while (true)
        {
            Score += 10;
            yield return new WaitForSeconds(0.5f);
            GameScreen.IncrementScore(Score);

            if (IsGameOver) break;
        }
    }

    //TODO: Refactor for loadingManager
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}