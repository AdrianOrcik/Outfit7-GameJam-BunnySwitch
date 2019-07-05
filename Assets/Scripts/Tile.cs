using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Interactable
{
    public int position_Y { get; set; }
    public int position_X { get; set; }

    void Start()
    {
        position_Y = (int) transform.position.y;
        position_X = (int) transform.position.x;
    }
}