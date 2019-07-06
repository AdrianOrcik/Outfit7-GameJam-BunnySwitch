using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static int LAYER_WIDTH = 6;

    public static float DEACTIVE_LAYER_ALPHA = 0.2f;
    public static float ACTIVE_LAYER_ALPHA = 1f;

    public static float DEACTIVE_LAYER_OFFSET = 0.8f;
    public static float ACTIVE_LAYER_OFFSET = 0f;

    public static float LAYER_MOVE_DURATION = 0.25f;

    public static float SPRITE_FADE_OUT_TIME = 0.5f;
    public static float SPRITE_FADE_IN_TIME = 0.1f;

    public static int DEACTIVE_SORTING_LAYER = 10;
    public static int ACTIVE_SORTING_LAYER = 15;

    public static string ObstacleLayer = "Obstacle";
    public static string TileLayer = "Tile";
    public static string EmptyTileLayer = "EmptyTile";
    public static string MushroomBounce = "Bounce";

    public static string PlayerRunAnimation = "Run";
    public static string PlayerDieObstacleAnimation = "DieObstacle";
    public static string PlayerDieFallAnimation = "DieFall";
    public static string PlayerJumpUp = "JumpUp";
    public static string PlayerJumpDown = "JumpDown";

    public static float GET_READY_TIME = 0.4f;
    public static float FADE_OUT_READY_SCREEN_TIME = 1.7f;

    public static float OBSTACLE_BOUCE_WAIT_TIME = 0.25f;
    public static float OBSTACLE_BOUCE_UP_TIME = 0.18f;
    public static float OBSTACLE_BOUCE_DOWN_TIME = 0.13f;

    public static float PLAYER_TRAMPOLINE_JUMP_UP_TIME = 0.5f;
    public static float PLAYER_TRAMPOLINE_JUMP_DOWN_LONG = 0.5f;
    public static float PLAYER_TRAMPOLINE_JUMP_DOWN_TIME = 0.2f;

    public static float PARALLAX_BACKGROUD_WIDTH = 20f;
    public static float PARALLAX_CLOUD_SPEED = -0.1f;
    public static float PARALLAX_BACK_LAYER_SPEED = 0.3f;
}