using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MainBehaviour
{
    public Action OnGameOver;
    public Action OnStartGame;
    public bool IsPlaying { get; set; }
    public PlayerManager PlayerManager { get; set; }

    private void Awake()
    {
        MainModel.GameManager = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        OnGameOver += GameOver;
        OnStartGame += StartGame;
    }

    private void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        IsPlaying = false;
        FindObjectOfType<PlayerManager>().Animator.SetBool(Constants.PlayerRunAnimation, false);

        yield return new WaitForSeconds(1f);
        ScreenManager.GetScreen<GameOverScreen>().gameObject.SetActive(true);
    }

    private void StartGame()
    {
        IsPlaying = true;
        PlayerManager = FindObjectOfType<PlayerManager>();
        PlayerManager.Animator.SetBool(Constants.PlayerRunAnimation, true);
    }

    //TODO: Refactor for loadingManager
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
}