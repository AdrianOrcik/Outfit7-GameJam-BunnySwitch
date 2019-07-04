using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
//    public GameDefinitions Definitions;
//    public GameDataLoader GameDataLoader;
//    public LoadSceneManager LoadSceneManager;
//    public EventManager EventManager;
//    public ResourceManager ResourceManager;
//
//    public bool InteractionEnabled;
//    public bool InteractionCameraMove;
//
//    public DateTime ServerTime
//    {
//        get { return DateTime.Now; }
//    }
//
//    public void Awake()
//    {
//        MainModel.GameManager = this;
//        InteractionEnabled = true;
//        SceneManager.activeSceneChanged += OnSceneChanged;
//        DontDestroyOnLoad(gameObject);
//    }
//
//    private void OnSceneChanged(Scene arg0, Scene arg1)
//    {
//        InteractionEnabled = true;
//    }
//
//    void Start()
//    {
//        Definitions.LoadDefinitions();
//        GameDataLoader.LoadGameData();
//        ResourceManager.LoadResources();
//        //EventManager.OnDefinitionsDownloaded();
//    }
//
//    private void OnApplicationPause(bool pause)
//    {
//        if (GameDataLoader != null)
//        {
//            GameDataLoader.SaveData();
//        }
//    }
//
//    private void OnApplicationQuit()
//    {
//        if (GameDataLoader != null)
//        {
//            GameDataLoader.SaveData();
//        }
//    }
//
//    private void OnDestroy()
//    {
//        if (GameDataLoader != null)
//        {
//            GameDataLoader.SaveData();
//        }
//    }
}