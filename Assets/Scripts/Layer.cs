using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Layer : MainBehaviour
{
    public bool IsActive { get; set; }
    public List<Interactable> Interactables = new List<Interactable>();

    void Start()
    {
        foreach (Transform tile in transform)
        {
            Interactables.Add(tile.GetComponent<Interactable>());
        }
    }

    public void SetTransparent(bool toActive)
    {
        foreach (Interactable interactable in Interactables)
        {
            interactable.SetTransparent(toActive);
        }
    }

}