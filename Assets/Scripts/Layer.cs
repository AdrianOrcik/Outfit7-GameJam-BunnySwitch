using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Layer : MainBehaviour
{
    public bool IsActive { get; set; }
    public LayerBlock LayerBlock { get; set; }
    public List<Interactable> Interactables = new List<Interactable>();

    void Start()
    {
        foreach (Transform tile in transform)
        {
            Interactable interactable = tile.GetComponent<Interactable>();
            interactable.Layer = this;
            Interactables.Add(interactable);
        }
    }

    public void SetTransparent(bool toActive)
    {
        foreach (Interactable interactable in Interactables)
        {
            Tile tile = interactable as Tile;
            if (tile != null)
            {
                tile.SetTileSprite(toActive);
                interactable.SetTransparent(toActive);
            }
            else
            {
                interactable.SetTransparent(toActive);
            }
        }
    }

    public void SetPrediction(Vector2 position)
    {
        foreach (Interactable interectable in Interactables)
        {
            if (interectable.SpriteRenderer != null)
            {
                interectable.SpriteRenderer.color = new Color(interectable.SpriteRenderer.color.r,
                    interectable.SpriteRenderer.color.g, interectable.SpriteRenderer.color.b, 0.5f);
            }
        }

        foreach (Interactable interectable in Interactables)
        {
            Tile tile = interectable as Tile;
            if (tile != null)
            {
                if (Math.Abs(tile.position_X - position.x) < 0.4f && Math.Abs(tile.position_Y - position.y) < 0.4f)
                {
                    tile.SpriteRenderer.color = new Color(interectable.SpriteRenderer.color.r,
                        interectable.SpriteRenderer.color.g, interectable.SpriteRenderer.color.b, 1f);
                }
            }
        }
    }

    private void OnDisable()
    {
        transform.position = new Vector3(0, 0, 0);
    }

//    public int GetYposition(Transform transform)
//    {
//        List<int> tilePositions
//        foreach (Interactable interactable in Interactables)
//        {
//            Tile tile = interactable.GetComponent<Tile>();
//            if (tile)
//            {
//                if (Math.Abs(transform.position.x - tile.position_X) < 0.2f)
//                {
//                    
//                }
//            }
//        }
//    }
}