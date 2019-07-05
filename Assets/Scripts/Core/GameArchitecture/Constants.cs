using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float DEACTIVE_LAYER_ALPHA = 0.2f;
    public static float ACTIVE_LAYER_ALPHA = 1f;

    public static float DEACTIVE_LAYER_OFFSET = 0.5f;
    public static float ACTIVE_LAYER_OFFSET = 0f;

    public static float LAYER_MOVE_DURATION = 0.25f;

    public static float SPRITE_FADE_OUT_TIME = 0.5f;
    public static float SPRITE_FADE_IN_TIME = 0.1f;

    public static int DEACTIVE_SORTING_LAYER = 10;
    public static int ACTIVE_SORTING_LAYER = 15;

    public static string ObstacleLayer = "Obstacle";
    public static string TileLayer = "Tile";
    public static string EmptyTileLayer = "EmptyTile";
}