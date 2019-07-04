using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MainBehaviour
{
    public bool IsActive { get; set; }
    public List<Tile> Tiles = new List<Tile>();

    void Start()
    {
        foreach (Transform tile in transform)
        {
            Tiles.Add(tile.GetComponent<Tile>());
        }
    }

    public void SetTransparent(bool toDeactive)
    {
        foreach (Tile tile in Tiles)
        {
            tile.SetTransparent(toDeactive);
        }
    }
}