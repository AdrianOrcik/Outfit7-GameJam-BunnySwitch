using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public class LevelEditorWindow : EditorWindow
{
    //public PathDefinition[] PathDefinitions;
    public ButtonsPanelEditor buttonsPanelEditor;
    public PredictionViewEditor predictionViewEditor;

    [MenuItem("TaterTools/Level Editor")]
    public static void ShowWindow()
    {
        LevelEditorWindow window =
            (LevelEditorWindow) GetWindow(typeof(LevelEditorWindow), true, "Level Editor");
        window.Show();
    }

    //Picked Item
    public GridType gridType = GridType.Tiles;
    public int selectedObj;

    //Texture Arrays
    public Texture[] Obstacles;
    public Texture[] Tiles;

    //CellSettings
    public List<GridObj> TileObjs = new List<GridObj>();
    public List<GridObj> ObstacleObjs = new List<GridObj>();
    public Vector2 GridCount = new Vector2(8, 3);

    //LevelName
    public string LevelName = "";

    //OnEnableGUI
    public bool IsPopupActive;
    public bool IsLevelSaved;
    private bool OnEnableGUI = true;

    //List of end points
    public List<Vector2> EndPoints;

    private LevelData LevelData;

    /// <summary>
    /// Call when Window is open and load tile textures from folder.
    /// Add tmpPath for TmpSavedData level from editor
    /// </summary>
    private void Init()
    {
//        PathDefinitions = LoadJson<PathDefinition[]>(string.Format("{0}/Resources/Definitions/PathDefinition.json",
//            Application.dataPath));

        if (buttonsPanelEditor == null) buttonsPanelEditor = new ButtonsPanelEditor(this);
        if (predictionViewEditor == null) predictionViewEditor = new PredictionViewEditor(this);

        buttonsPanelEditor.Init();
        buttonsPanelEditor.buttonSelected = (int) gridType;

        Obstacles = (Resources.LoadAll("LevelEditor/Obstacles/", typeof(Texture))).Cast<Texture>().ToArray();
        Tiles = (Resources.LoadAll("Articles/Path/", typeof(Texture))).Cast<Texture>().ToArray();

        GUI.color = Color.white;
    }

    /// <summary>
    /// Called once per frame and draw all window
    /// </summary>
    private void OnGUI()
    {
        if (OnEnableGUI)
        {
            Init();
            OnEnableGUI = false;
        }

        GUILayout.BeginVertical();
        buttonsPanelEditor.MainButtonsPanel();

        MainGridPanel();

        buttonsPanelEditor.SelectButtonsPanel();
        GUILayout.EndVertical();

        if (!IsPopupActive)
        {
            predictionViewEditor.MousePositionDraw();
        }

        Repaint();
    }

    /// <summary>
    /// Draw main grid with obstacles
    /// </summary>
    private void MainGridPanel()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width, (GridCount.y * Constants.CELL_SIZE) + 100));
        EditorGUILayout.BeginHorizontal();

        //Responsive grid scroller
        if (GridCount.y * Constants.CELL_SIZE <= position.height / 2.0)
        {
            buttonsPanelEditor.gridScrollPos = EditorGUILayout.BeginScrollView(buttonsPanelEditor.gridScrollPos,
                GUILayout.Width(position.width),
                GUILayout.Height((GridCount.y * Constants.CELL_SIZE) + Constants.CELL_SIZE));
        }
        else
        {
            buttonsPanelEditor.gridScrollPos = EditorGUILayout.BeginScrollView(buttonsPanelEditor.gridScrollPos,
                GUILayout.Width(position.width),
                GUILayout.Height(position.height / 1.9f + Constants.CELL_SIZE));
        }

        if (gridType == GridType.Tiles)
        {
            DrawMatrix(TileObjs, Tiles, GridType.Obstacles, gridType);
        }
        else
        {
            DrawMatrix(ObstacleObjs, Obstacles, GridType.Tiles, gridType);
        }

        if (buttonsPanelEditor.GroupLayers)
        {
            RefreshAllLayersMatrix();
        }

        //Horizontal - space printing in horizontal axes
        EditorGUILayout.BeginHorizontal();
        for (int x = 0; x < GridCount.x; x++)
        {
            GUILayout.Space(Constants.CELL_SIZE);
        }

        EditorGUILayout.EndHorizontal();

        //Vertical doesnt need vertical layer - space printing in vertical axes 
        for (int y = 0; y < GridCount.y; y++)
        {
            GUILayout.Space(Constants.CELL_SIZE);
        }

        GUILayout.Space(Constants.CELL_SIZE);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    /// <summary>
    /// Draw full matrix with teture element and add buttons listener
    /// Drawing multiTiles & singleTiles
    /// </summary>
    /// <param name="objsData">Tiles or obstacle data</param>
    /// <param name="textures">Textures from foldes </param>
    /// <param name="alphaBlendGridType">Which layer is alphaBlended</param>
    /// <param name="currentGridType">Current layer</param>
    private void DrawMatrix(List<GridObj> objsData, Texture[] textures, GridType alphaBlendGridType,
        GridType currentGridType)
    {
        if (objsData.Count == 0) return;

        for (int i = 0; i < objsData.Count; i++)
        {
            GUI.Box(objsData[i].GUIRect, "");

            DrawTextureOnCell(objsData[i], textures, alphaBlendGridType);

            if (GUI.Button(objsData[i].GUIRect, "", GUIStyle.none))
            {
                if (predictionViewEditor.OutOfGrid) return;

                if (IsMultiple())
                {
                    for (int j = 0; j < predictionViewEditor.OffsetPos.Count; j++)
                    {
                        Vector2 objPos = new Vector2(
                            (objsData[i].GUIRect.x + (predictionViewEditor.OffsetPos[j].x * Constants.CELL_SIZE)) /
                            Constants.CELL_SIZE,
                            ((objsData[i].GUIRect.y + (predictionViewEditor.OffsetPos[j].y * Constants.CELL_SIZE)) /
                             Constants.CELL_SIZE) - 1);

                        int position = ((int) objPos.y * (int) GridCount.x) + ((int) objPos.x + (int) objPos.y) -
                                       (int) objPos.y;

                        SetObstacleData(objsData[position], currentGridType,
                            GetSelectedObjID(predictionViewEditor.ArticleSubTiles[j]));
                    }
                }
                else
                {
                    SetObstacleData(objsData[i], currentGridType);
                }
            }
        }
    }

    /// <summary>
    /// Find index of texture in textureArray
    /// </summary>
    /// <param name="tileID"></param>
    /// <returns></returns>
    private int GetSelectedObjID(int tileID)
    {
        int onIndex = 0;
        foreach (Texture tile in Tiles)
        {
            if (String.CompareOrdinal(tile.name, tileID + "_icon") == 0)
            {
                return onIndex;
            }

            onIndex++;
        }

        Debug.Log("Tile texture has not been in folder !");
        return 0;
    }

    /// <summary>
    /// Set Obstacle Data to Cell
    /// Remove Obstacle if RemoveCell is enable
    /// </summary>
    /// <param name="cell">Concrete cell from grid</param>
    /// <param name="currentType">Selected GridType</param>
    private void SetObstacleData(GridObj cell, GridType currentType, int? selectedObjID = null)
    {
        if (buttonsPanelEditor.selectedOptionIcon == (int) OptionIconType.Remove)
        {
            cell.ObstacleIDint = -1;
            cell.ObstacleIDstring = null;
            return;
        }

        if (gridType == currentType && gridType == GridType.Tiles)
        {
            cell.ObstacleIDint = selectedObjID ?? selectedObj;
            cell.ObstacleIDstring = Tiles[selectedObjID ?? selectedObj].name;
        }
        else
        {
            cell.ObstacleIDint = selectedObj;
            cell.ObstacleIDstring = Obstacles[selectedObj].name;
        }
    }

    /// <summary>
    /// Check if tile is multiple
    /// </summary>
    /// <param name="selectedObjID"></param>
    /// <returns></returns>
    private bool IsMultiple()
    {
        return predictionViewEditor.ArticleSubTiles.Count > 0;
    }

    /// <summary>
    /// Use when GroupLayer function is enabled, merge to draw all layers.
    /// </summary>
    private void RefreshAllLayersMatrix()
    {
        for (int i = 0; i < TileObjs.Count; i++)
        {
            DrawTextureOnCell(ObstacleObjs[i], Obstacles, GridType.Tiles);
            DrawTextureOnCell(TileObjs[i], Tiles, GridType.Obstacles);
        }
    }

    /// <summary>
    /// Draw img texture on concrete grid
    /// </summary>
    /// <param name="cell">Concrete Cell for drawing</param>
    /// <param name="textures">Images from folder</param>
    /// <param name="alphaBlendType">Which layer do you want alphablend</param>
    private void DrawTextureOnCell(GridObj cell, Texture[] textures, GridType alphaBlendType)
    {
        Color tmpColor;
        if (cell.ObstacleIDint > -1)
        {
            if (buttonsPanelEditor.GroupLayers && gridType == alphaBlendType)
            {
                tmpColor = new Color(1f, 1f, 1f, 0.25f);
                GUI.color = tmpColor;
            }

            if (buttonsPanelEditor.selectedOptionIcon == (int) OptionIconType.Remove)
            {
                tmpColor = new Color(1f, 0.01f, 0f, 0.59f);
                GUI.color = tmpColor;
            }

            GUI.DrawTexture(cell.GUIRect, textures[cell.ObstacleIDint], ScaleMode.StretchToFill,
                true, 11.0F);
        }

        tmpColor = new Color(1f, 1f, 1f, 1f);
        GUI.color = tmpColor;
    }

    /// <summary>
    /// Generate Matrix with allocate cells for layers
    /// </summary>
    public void GenerateMatrix()
    {
        TileObjs.Clear();
        ObstacleObjs.Clear();

        for (int yPos = 0; yPos < GridCount.y; yPos++)
        {
            for (int xPos = 0; xPos < GridCount.x; xPos++)
            {
                //Tiles - layers1
                GridObj objT = new GridObj();
                objT.MatrixRect = new Rect(xPos * Constants.CELL_SIZE,
                    yPos * Constants.CELL_SIZE + Constants.CELL_PADDING, Constants.CELL_SIZE, Constants.CELL_SIZE);
                objT.GUIRect = new Rect(objT.MatrixRect.position.x, objT.MatrixRect.position.y, Constants.CELL_SIZE,
                    Constants.CELL_SIZE);
                GUI.Box(objT.GUIRect, "");
                TileObjs.Add(objT);

                //Obstacles - layers2
                GridObj objO = new GridObj();
                objO.MatrixRect = new Rect(xPos * Constants.CELL_SIZE,
                    yPos * Constants.CELL_SIZE + Constants.CELL_PADDING, Constants.CELL_SIZE, Constants.CELL_SIZE);
                objO.GUIRect = new Rect(objO.MatrixRect.position.x, objO.MatrixRect.position.y, Constants.CELL_SIZE,
                    Constants.CELL_SIZE);
                GUI.Box(objO.GUIRect, "");
                ObstacleObjs.Add((objO));
            }
        }

        if (GridCount.y * Constants.CELL_SIZE <= position.height / 2f)
        {
            buttonsPanelEditor.bottomPanelPadding = ((int) GridCount.y * Constants.CELL_SIZE) + Constants.CELL_SIZE;
        }
        else
        {
            buttonsPanelEditor.bottomPanelPadding = (int) (position.height / 1.8f) + Constants.CELL_SIZE;
        }
    }

    //Close Window events
    private void OnDestroy()
    {
        if (EditorUtility.DisplayDialog("Warning!", "Your unsaved level will be remove", "Save", "Exit"))
        {
            buttonsPanelEditor.SaveLevel();
        }
    }

    private void OnLostFocus()
    {
        buttonsPanelEditor.LostFocusSaveLevel();
    }
//
//    //Check handle methods
//    public bool HasDoors()
//    {
//        bool spawnPoint = false;
//        bool endPoint = false;
//        int length = (int) GridCount.x * (int) GridCount.y;
//        EndPoints = new List<Vector2>();
//        for (int i = 0; i < length; i++)
//        {
//            if (TileObjs[i].ObstacleIDstring == null || TileObjs[i].ObstacleIDint == -1) continue;
//
//            if (ObstacleObjs[i].ObstacleIDstring != null && ObstacleObjs[i].ObstacleIDint != -1)
//            {
//                if ((int) ArticleCategory.SpawnPoint == Int32.Parse(ObstacleObjs[i].ObstacleIDstring) ||
//                    (int) ArticleCategory.SpawnPointDown == Int32.Parse(ObstacleObjs[i].ObstacleIDstring))
//                {
//                    spawnPoint = true;
//                }
//
//                if ((int) ArticleCategory.EndPoint == Int32.Parse(ObstacleObjs[i].ObstacleIDstring) ||
//                    (int) ArticleCategory.EndPointDown == Int32.Parse(ObstacleObjs[i].ObstacleIDstring))
//                {
//                    endPoint = true;
//                    int x = ((int) ObstacleObjs[i].GUIRect.x / 50) + 1; //X grid indexed from zero
//                    int y = (int) ObstacleObjs[i].GUIRect.y / 50;
//                    EndPoints.Add(new Vector2(x, y));
//                }
//            }
//        }
//
//        return spawnPoint && endPoint;
//    }

    /// <summary>
    /// Load json file, convert to LevelDef. type and allocate data to Objs
    /// </summary>
    /// <param name="path">Path to load file</param>
    public void LoadToLevelData(string path)
    {
        LevelData = LoadJson<LevelData>(path);
        if (LevelData.LevelDefinition.Count == 0 || LevelData.LevelDefinition == null)
        {
            Debug.LogError("Loading -- LevelData is null!");
            return;
        }

        GridCount.x = LevelData.LevelInGameDefinition.xCount;
        GridCount.y = LevelData.LevelInGameDefinition.yCount;
        LevelName = LevelData.LevelInGameDefinition.LevelName;

        GenerateMatrix();

        foreach (var data in LevelData.LevelDefinition)
        {
            int positionY = ((data.Position_Y - (int) GridCount.y + 1) * -1);
            int positionX = data.Position_X;
            int position = ((positionY * ((int) GridCount.x - 1)) + ((positionX) + (positionY)));

            if (data.EntityIDLayer1 != 0)
            {
                TileObjs[position].ObstacleIDint = data.Layer1EditorID;
                TileObjs[position].ObstacleIDstring = data.EntityIDLayer1.ToString();
            }

            if (data.EntityIDLayer2 != 0)
            {
                ObstacleObjs[position].ObstacleIDint = data.Layer2EditorID;
                ObstacleObjs[position].ObstacleIDstring = data.EntityIDLayer2.ToString();
            }
        }
    }

    /// <summary>
    /// Convert level data to correct x,y position and make LevelData for saving
    /// </summary>
    /// <returns>Converted Level Data</returns>
    public LevelData SaveToLevelData()
    {
        LevelData.LevelDefinition = new List<LevelDefinition>();
        int length = (int) GridCount.x * (int) GridCount.y;

        int maxXposition = -9999;

        for (int i = 0; i < length; i++)
        {
            //Trap or obstacle can be use only on waypoint tile,
            //is tile has not waypoint continue to next tile
            if (TileObjs[i].ObstacleIDstring == null || TileObjs[i].ObstacleIDint == -1) continue;

            var data = new LevelDefinition();
            int yPos = (int) (TileObjs[i].MatrixRect.position.y / Constants.CELL_SIZE);
            int xPos = (int) (TileObjs[i].MatrixRect.position.x / Constants.CELL_SIZE);

            data.Position_Y = (yPos - (int) GridCount.y) * -1;
            data.Position_X = xPos;

            string pathID;
            if (TileObjs[i].ObstacleIDstring.Length > Constants.ID_CODE_LENGTH)
            {
                pathID = TileObjs[i].ObstacleIDstring.Remove(Constants.ID_CODE_LENGTH);
            }
            else
            {
                pathID = TileObjs[i].ObstacleIDstring;
            }

            data.EntityIDLayer1 = Int32.Parse(pathID);
            data.Layer1EditorID = TileObjs[i].ObstacleIDint;

            if (ObstacleObjs[i].ObstacleIDstring != null && ObstacleObjs[i].ObstacleIDint != -1)
            {
                data.EntityIDLayer2 = Int32.Parse(ObstacleObjs[i].ObstacleIDstring);
                data.Layer2EditorID = ObstacleObjs[i].ObstacleIDint;
            }

            if (data.Position_X > maxXposition)
            {
                maxXposition = data.Position_X;
            }

            LevelData.LevelDefinition.Add(data);
        }

        if (LevelData.LevelInGameDefinition != null)
        {
            LevelData.LevelInGameDefinition = LevelData.LevelInGameDefinition;
        }
        else
        {
            LevelData.LevelInGameDefinition = new LevelInGameDefinition();
        }

        LevelData.LevelInGameDefinition.xCount = (int) GridCount.x;
        LevelData.LevelInGameDefinition.yCount = (int) GridCount.y;
        LevelData.LevelInGameDefinition.MaxTileX = maxXposition + 1;
        LevelData.LevelInGameDefinition.LevelName = LevelName;

        return LevelData;
    }

    public EditorData SaveToEditorData()
    {
        EditorData editorData = new EditorData();
        editorData.EndPoints = EndPoints;

        return editorData;
    }

    //Json methods
    public void SaveJson(string fileName, object saveData)
    {
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        string path = string.Format("{0}/Resources/WIPLevels/{1}.json", Application.dataPath, fileName);
        Debug.Log(path);
        File.WriteAllText(path, json);
        AssetDatabase.ImportAsset("Assets/Resources/WIPLevels/" + fileName + ".json");
    }

    private T LoadJson<T>(string path)
    {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json);
    }
}