using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class SaveEditorWindow : EditorWindow
{
    private static LevelEditorWindow levelEditor;
    private static object levelDefinitionData;
    private static EditorData editorData;
    private string tmpFileName = "";
    private int remainingNunu = 10;
    private int remainingWaves = 1;
    private int goalNunu = 5;
    private List<int> goalSpawnWave;
    private List<int> endPointsCapacity;
    private List<int> spawnWaveCapacity;

    private Vector2 DefaultSizeWindow = new Vector2(500f, 170f);
    private static EditorWindow window;
    private LevelData levelData;

    public static void ShowWindow(LevelEditorWindow _levelEditorWindow, EditorData _editorData,
        object _levelDefinitionData)
    {
        window = GetWindow<SaveEditorWindow>("Save", true);
        window.maxSize = new Vector2(500f, 150f);
        window.minSize = window.maxSize;
        window.Show();

        levelEditor = _levelEditorWindow;
        levelDefinitionData = _levelDefinitionData;
        editorData = _editorData;

        levelEditor.IsPopupActive = true;
    }

//    private void OnEnable()
//    {
//        Debug.Log("OnEnable");
//        DefaultInit();
//    }

    private void LoadLevelVariablesData()
    {
        remainingNunu = LoadVariableData(levelData.LevelInGameDefinition.RemainingNunu, remainingNunu);
        remainingWaves = LoadVariableData(levelData.LevelInGameDefinition.RemainingWaves, remainingWaves);
        goalNunu = LoadVariableData(levelData.LevelInGameDefinition.GoalNunu, goalNunu);

        if (levelData.LevelInGameDefinition.SpawnWaves != null)
        {
            goalSpawnWave = levelData.LevelInGameDefinition.SpawnWaves;
        }
    }

    private int LoadVariableData(int fromVariable, int toVariable)
    {
        if (fromVariable != 0)
        {
            return fromVariable;
        }

        return toVariable;
    }

    private void DefaultInit()
    {
        levelData = (LevelData) levelDefinitionData;
        int fileID = Directory.GetFiles(Application.dataPath + "/" + "Resources/WIPLevels" + "/", "level_*.json",
            SearchOption.TopDirectoryOnly).Length;

        if (string.IsNullOrEmpty(levelData.LevelInGameDefinition.LevelName))
        {
            tmpFileName = string.Format("level_{0}", fileID + 1);
        }
        else
        {
            tmpFileName = levelData.LevelInGameDefinition.LevelName;
        }

        endPointsCapacity = new List<int>();
        spawnWaveCapacity = new List<int>();
        goalSpawnWave = new List<int>();
        LoadLevelVariablesData();
    }

    private bool DetectRewriteLevel()
    {
        string[] fileName = Directory.GetFiles(Application.dataPath + "/" + "Resources/WIPLevels" + "/",
            tmpFileName + ".json",
            SearchOption.TopDirectoryOnly);
        return fileName.Length > 0;
    }

    private void OnGUI()
    {
        if (levelDefinitionData == null) Debug.Log("NULL!");

        if (goalSpawnWave == null)
        {
            DefaultInit();
        }

        GUILayout.Label("InGame Settings");
        GUI_LevelSettings();
        GUILayout.FlexibleSpace();
        GUI_RemainingWaves();
        GUILayout.FlexibleSpace();
        GUI_RemainingNunu();
        GUILayout.FlexibleSpace();

        GUILayout.Label("__Level Name");
        tmpFileName = EditorGUILayout.TextField("File Name:", tmpFileName);
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Save"))
        {
            SaveLevel();
            Close();
        }

        window.maxSize = new Vector2(DefaultSizeWindow.x,
            DefaultSizeWindow.y + (editorData.EndPoints.Count + remainingWaves) * Constants.HEIGHT_LINE);
        window.minSize = window.maxSize;
    }

    private void GUI_LevelSettings()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Remaining Waves:");
        remainingWaves = EditorGUILayout.IntField("", remainingWaves, GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Remaining Nunu:");
        remainingNunu = EditorGUILayout.IntField("", remainingNunu, GUILayout.Width(100));
        GUILayout.EndHorizontal();
    }

    private void GUI_RemainingWaves()
    {
        GUILayout.Label("__Wave Settings");
        spawnWaveCapacity.Clear();
        for (int i = 0; i < remainingWaves; i++)
        {
            if (goalSpawnWave.Count - 1 < remainingWaves)
            {
                goalSpawnWave.Add(0);
            }
            
            if (remainingWaves == 1)
            {
                goalSpawnWave[i] = remainingNunu;
            }



            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("Wave Count[{0}]: ", i));
            goalSpawnWave[i] = EditorGUILayout.IntField("", goalSpawnWave[i], GUILayout.Width(100));
            spawnWaveCapacity.Add(goalSpawnWave[i]);
            GUILayout.EndHorizontal();
        }
    }

    private void GUI_RemainingNunu()
    {
        GUILayout.Label("__EscortGate Settings");
        endPointsCapacity.Clear();
        for (int i = 0; i < editorData.EndPoints.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("EndPoint[{0},{1}]: ", editorData.EndPoints[i].x, editorData.EndPoints[i].y));
            goalNunu = EditorGUILayout.IntField("", remainingNunu / editorData.EndPoints.Count, GUILayout.Width(100));
            endPointsCapacity.Add(goalNunu);
            GUILayout.EndHorizontal();
        }
    }

    private void SaveLevel()
    {
        if (DetectRewriteLevel())
        {
            if (EditorUtility.DisplayDialog("Warning!", "Do you want rewrite your level ?", "Exit", "Save"))
            {
                return;
            }
        }


        levelData.LevelInGameDefinition.GoalNunu = goalNunu;
        levelData.LevelInGameDefinition.RemainingNunu = remainingNunu;
        levelData.LevelInGameDefinition.RemainingWaves = remainingWaves;
        levelData.LevelInGameDefinition.EndPointsCapacity = endPointsCapacity;
        levelData.LevelInGameDefinition.SpawnWaves = spawnWaveCapacity;

        string fileName = tmpFileName;
        levelData.LevelInGameDefinition.LevelName = fileName;

        if (levelDefinitionData == null) Debug.LogError("Data is null");
        SaveJson(fileName, levelData);
        SaveJson(levelEditor.buttonsPanelEditor.tmpLevelName, levelData);
        Debug.Log(string.Format("--File has been saved with name {0}.json--", fileName));
    }

    private void SaveJson(string fileName, object saveData)
    {
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        string path = string.Format("{0}/Resources/WIPLevels/{1}.json", Application.dataPath, fileName);

        File.WriteAllText(path, json);
        AssetDatabase.ImportAsset("Assets/Resources/WIPLevels/" + fileName + ".json");
    }

    private void OnDestroy()
    {
        levelEditor.IsPopupActive = false;
    }
}