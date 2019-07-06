using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Interactable
{
    public Sprite ActiveTile;
    public Sprite DeactiveTile;

    public int position_Y { get; set; }
    public int position_X { get; set; }

    void Start()
    {
        position_Y = (int) transform.position.y;
        position_X = (int) transform.position.x;
    }

    public void SetTileSprite(bool isActive)
    {
        if (isActive)
        {
            SpriteRenderer.sprite = ActiveTile;
        }
        else
        {
            SpriteRenderer.sprite = DeactiveTile;
        }
    }
}