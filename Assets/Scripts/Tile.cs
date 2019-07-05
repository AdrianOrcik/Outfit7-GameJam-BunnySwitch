using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Interactable
{
    public int position_Y { get; set; }

    void Start()
    {
        position_Y = (int) transform.position.y;
    }
}