using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using UnityEngine;
using UnityEngine.Analytics;

public class PredictionViewEditor
{
    private LevelEditorWindow levelEditor;

    //Colors
    private Color GreenAlphaColor = new Color(0f, 1f, 0.09f, 0.5f);
    private Color RedAlphaColor = new Color(1f, 0.01f, 0f, 0.50f);

    //MousePosition
    private Event guiEvent;

    public PredictionViewEditor(LevelEditorWindow levelEditor)
    {
        this.levelEditor = levelEditor;
        guiEvent = Event.current;
    }

    //SubTiles for multi tiles
    public List<int> ArticleSubTiles;

    //Offsets for subTiles
    public List<Vector2> OffsetPos;

    public bool OutOfGrid;

    /// <summary>
    /// Calculating where the mouse on editor screen is located
    /// </summary>
    public void MousePositionDraw()
    {
        Vector2 mousePos = GetMousePosition();

        if (levelEditor.TileObjs == null || levelEditor.TileObjs.Count == 0) return;

        int positionID = (int) mousePos.y * ((int) levelEditor.GridCount.x - 1) +
                         (int) mousePos.x + ((int) mousePos.y) -
                         (int) levelEditor.GridCount.x;

        if ((positionID >= levelEditor.TileObjs.Count || positionID >= levelEditor.ObstacleObjs.Count) ||
            (positionID < 0))
        {
            return;
        }

        DrawPrediction(positionID);
    }

    /// <summary>
    /// Draw prediction for tiles & obstacles
    /// </summary>
    /// <param name="positionID"></param>
    private void DrawPrediction(int positionID)
    {
        if (levelEditor.gridType == GridType.Tiles)
        {
            ArticleSubTiles = GetPathInfo();

            if (OffsetPos.Count > 0)
            {
                PredictionMultiTile(positionID);
            }
            else
            {
                PredictionSingleTile(positionID);
            }
        }
        else
        {
            if (levelEditor.TileObjs[positionID].ObstacleIDint != -1 && !HasDoors(positionID))
            {
                GUI.color = GreenAlphaColor;
            }
            else
            {
                GUI.color = RedAlphaColor;
            }

            DrawPredictionOnCell(positionID, levelEditor.Obstacles);
        }

        GUI.color = Color.white;
    }

    /// <summary>
    /// Calculating draw prediction with multiTile
    /// </summary>
    /// <param name="positionID"></param>
    private void PredictionMultiTile(int positionID)
    {
        for (int i = 0; i < OffsetPos.Count; i++)
        {
            Vector2 objPosMulti = new Vector2(
                (levelEditor.TileObjs[positionID].GUIRect.x + (OffsetPos[i].x * Constants.CELL_SIZE)) /
                Constants.CELL_SIZE,
                ((levelEditor.TileObjs[positionID].GUIRect.y + (OffsetPos[i].y * Constants.CELL_SIZE)) /
                 Constants.CELL_SIZE) - 1);

            OutOfGrid = CheckOutOfGrid(objPosMulti);

            int positionMulti = ((int) objPosMulti.y * (int) levelEditor.GridCount.x) +
                                ((int) objPosMulti.x + (int) objPosMulti.y) -
                                (int) objPosMulti.y;

            if (!OutOfGrid)
            {
                if (positionMulti < levelEditor.TileObjs.Count &&
                    levelEditor.TileObjs[positionMulti].ObstacleIDint == -1)
                {
                    GUI.color = GreenAlphaColor;
                }
                else
                {
                    GUI.color = RedAlphaColor;
                }
            }
            else
            {
                GUI.color = RedAlphaColor;
            }

            DrawPredictionOnCellMultiple(positionID, levelEditor.Tiles, ArticleSubTiles);
        }
    }

    /// <summary>
    /// Calculating draw prediction with singleTile
    /// </summary>
    /// <param name="positionID"></param>
    private void PredictionSingleTile(int positionID)
    {
        OutOfGrid = false;
        if (levelEditor.TileObjs[positionID].ObstacleIDint == -1)
        {
            GUI.color = GreenAlphaColor;
        }
        else
        {
            GUI.color = RedAlphaColor;
        }

        DrawPredictionOnCell(positionID, levelEditor.Tiles);
    }

    /// <summary>
    /// Check if selected tile is out of grid 
    /// </summary>
    /// <param name="objPosMulti"></param>
    /// <returns></returns>
    private bool CheckOutOfGrid(Vector2 objPosMulti)
    {
        return objPosMulti.x >= levelEditor.GridCount.x || objPosMulti.y >= levelEditor.GridCount.y;
    }

    /// <summary>
    /// Get subtiles and parse subMultiTiles offset into vector2 
    /// </summary>
    /// <returns></returns>
    private List<int> GetPathInfo()
    {
//        OffsetPos = new List<Vector2>();
//        if (levelEditor.gridType == GridType.Tiles && levelEditor.TileObjs.Count > 0)
//        {
//            //Found Root object
//            int id = Convert.ToInt32(levelEditor.Tiles[levelEditor.selectedObj].name.Remove(6));
//            List<int> subTilesList = new List<int>();
//
//            //Found and get subTiles into list
//            foreach (PathDefinition p in levelEditor.PathDefinitions)
//            {
//                if (p.RootID == id && p.RootOffset.Length > 1)
//                {
//                    subTilesList.Add(p.ArticleID);
//
//                    //Parse subtiles offset
//                    List<string> stringOffset = new List<string>();
//                    stringOffset = p.RootOffset.Split(',').ToList();
//
//                    OffsetPos.Add(new Vector2(Convert.ToInt32(stringOffset[0]),
//                        Convert.ToInt32(stringOffset[1])));
//                }
//            }
//
//            return subTilesList;
//        }

        return new List<int>();
    }

    /// <summary>
    /// Check if obstacle is not droping on door
    /// </summary>
    /// <param name="positionID"></param>
    private bool HasDoors(int positionID)
    {
        if (levelEditor.ObstacleObjs[positionID].ObstacleIDstring == null) return false;

//        return (int) ArticleCategory.SpawnPoint == Int32.Parse(levelEditor.ObstacleObjs[positionID].ObstacleIDstring) ||
//               (int) ArticleCategory.EndPoint == Int32.Parse(levelEditor.ObstacleObjs[positionID].ObstacleIDstring);
        return true;
    }

    /// <summary>
    /// Draw singleTile on grid
    /// </summary>
    /// <param name="positionID"></param>
    /// <param name="textures"></param>
    private void DrawPredictionOnCell(int positionID, Texture[] textures)
    {
        GUI.DrawTexture(levelEditor.TileObjs[positionID].MatrixRect, textures[levelEditor.selectedObj],
            ScaleMode.StretchToFill,
            true, 11.0F);
    }

    /// <summary>
    /// Draw multiTile on grid
    /// </summary>
    /// <param name="positionID"></param>
    /// <param name="textures"></param>
    /// <param name="texturePositon"></param>
    private void DrawPredictionOnCellMultiple(int positionID, Texture[] textures, List<int> texturePositon)
    {
        Texture[] textureArr = new Texture[ArticleSubTiles.Count];
        int textureID = 0;
        foreach (var tex in textures)
        {
            if (Convert.ToInt32(tex.name.Remove(6)) == texturePositon[textureID])
            {
                textureArr[textureID] = tex;
                if (textureID + 1 < ArticleSubTiles.Count)
                {
                    textureID++;
                }
            }
        }

        for (int i = 0; i < texturePositon.Count; i++)
        {
            Rect r = new Rect(
                levelEditor.TileObjs[positionID].MatrixRect.x + (OffsetPos[i].x * Constants.CELL_SIZE),
                levelEditor.TileObjs[positionID].MatrixRect.y + (OffsetPos[i].y * Constants.CELL_SIZE),
                Constants.CELL_SIZE,
                Constants.CELL_SIZE);

            GUI.DrawTexture(r, textureArr[i], ScaleMode.StretchToFill, true, 11.0F);
        }
    }

    /// <summary>
    /// Get mouse position on grid as Vector2
    /// </summary>
    /// <returns></returns>
    private Vector2 GetMousePosition()
    {
        return new Vector2((int) guiEvent.mousePosition.x / Constants.CELL_SIZE,
            (int) guiEvent.mousePosition.y / Constants.CELL_SIZE);
    }
}