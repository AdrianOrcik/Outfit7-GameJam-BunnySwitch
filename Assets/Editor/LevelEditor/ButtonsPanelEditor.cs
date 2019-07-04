using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ButtonsPanelEditor
{
    private LevelEditorWindow levelEditor;

    public ButtonsPanelEditor(LevelEditorWindow levelEditor)
    {
        this.levelEditor = levelEditor;
    }

    //Const
    private const int optionIconsInRow = 5;
    private const int interactableIconsInRow = 6;

    //Texture array
    private Texture[] OptionIcons;

    //Featchures
    public int selectedOptionIcon = (int) OptionIconType.Create;
    public bool GroupLayers = true;

    //LoadLastLevel
    public string tmpLevelName = "TMP_DONT_REMOVE";
    private string tmpPath = "";
    public int bottomPanelPadding = 0;

    //Items
    public string[] buttonsName = {"EntityIDLayer1", "EntityIDLayer2"};
    public int buttonSelected;

    //Scrolls
    public Vector2 gridScrollPos;
    public Vector2 obstacleScrollPos;

    /// <summary>
    /// Init Data when editor is opened
    /// </summary>
    public void Init()
    {
        tmpPath = string.Format("{0}/Resources/WIPLevels/{1}.json", Application.dataPath, tmpLevelName);
        OptionIcons = (Resources.LoadAll("LevelEditor/OptionIcons/", typeof(Texture))).Cast<Texture>().ToArray();

        if (File.Exists(tmpPath))
        {
            LoadLastLevel();
        }
        else
        {
            levelEditor.GenerateMatrix();
        }
    }

    /// <summary>
    /// Draw top button panel
    /// </summary>
    public void MainButtonsPanel()
    {
        GUILayout.BeginArea(new Rect(0, 0, levelEditor.position.width, Constants.CELL_SIZE));

        GUILayout.BeginHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Size [X,Y]");
        levelEditor.GridCount.x = EditorGUILayout.IntField("", (int) levelEditor.GridCount.x, GUILayout.Width(30));
        levelEditor.GridCount.y = EditorGUILayout.IntField("", (int) levelEditor.GridCount.y, GUILayout.Width(30));
        if (GUILayout.Button("Generate")) levelEditor.GenerateMatrix();
        GUILayout.FlexibleSpace();
        GUILayout.Label("LevelName: " + levelEditor.LevelName);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GroupLayers = GUILayout.Toggle(GroupLayers, "GroupLayers");
        if (GUILayout.Button("Save")) SaveLevel();
        if (GUILayout.Button("Load")) LoadLevel();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    /// <summary>
    /// Draw layer buttons and select default entityLayer
    /// </summary>
    public void SelectButtonsPanel()
    {
        GUILayout.BeginArea(new Rect(0, bottomPanelPadding + 40, levelEditor.position.width, 500));

        GUILayout.Label("");
        GUILayout.BeginVertical();
        buttonSelected = GUILayout.SelectionGrid(buttonSelected, buttonsName, buttonsName.Length);
        GUILayout.EndVertical();

        obstacleScrollPos = EditorGUILayout.BeginScrollView(obstacleScrollPos,
            GUILayout.Width(levelEditor.position.width),
            GUILayout.Height(300));

        levelEditor.gridType = (GridType) buttonSelected;
        switch (levelEditor.gridType)
        {
            case GridType.Tiles:
                ShowTiles();
                break;
            case GridType.Obstacles:
                ShowObstacles();
                break;
        }

        GUILayout.Space(Constants.CELL_SIZE);
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    /// <summary>
    /// Show Tiles in bottom menu
    /// </summary>
    private void ShowTiles()
    {
        GUILayout.BeginVertical();
        selectedOptionIcon = GUILayout.SelectionGrid(selectedOptionIcon,
            OptionIcons, optionIconsInRow,
            GUILayout.Height(25),
            GUILayout.Width(400));
        levelEditor.selectedObj = GUILayout.SelectionGrid(levelEditor.selectedObj, levelEditor.Tiles,
            interactableIconsInRow, GUILayout.Height(200),
            GUILayout.Width(levelEditor.position.width));
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Show obstacles in bottom menu
    /// </summary>
    public void ShowObstacles()
    {
        GUILayout.BeginVertical();
        selectedOptionIcon = GUILayout.SelectionGrid(selectedOptionIcon,
            OptionIcons, optionIconsInRow,
            GUILayout.Height(25),
            GUILayout.Width(400));
        levelEditor.selectedObj = GUILayout.SelectionGrid(levelEditor.selectedObj, levelEditor.Obstacles,
            interactableIconsInRow, GUILayout.Height(200),
            GUILayout.Width(levelEditor.position.width));
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Save data by lost focus from window
    /// </summary>
    public void LostFocusSaveLevel()
    {
        object saveData = levelEditor.SaveToLevelData();
        levelEditor.SaveJson(tmpLevelName, saveData);
    }

    /// <summary>
    /// Loading last level when editor is opened
    /// </summary>
    public void LoadLastLevel()
    {
        levelEditor.LoadToLevelData(tmpPath);
    }

    public void SaveLevel()
    {
//        if (!levelEditor.HasDoors())
//        {
//            PopupAlertEditorWindow.ShowPopupWindow("SpawnPoint or EndPoint is missing!");
//            return;
//        }

        object saveData = levelEditor.SaveToLevelData();
        EditorData editorData = levelEditor.SaveToEditorData();
        SaveEditorWindow.ShowWindow(levelEditor, editorData, saveData);
    }

    private void LoadLevel()
    {
        string path = EditorUtility.OpenFilePanel("Levels", Application.dataPath + "/" + "Resources/WIPLevels", "json");
        levelEditor.LoadToLevelData(path);
    }
}