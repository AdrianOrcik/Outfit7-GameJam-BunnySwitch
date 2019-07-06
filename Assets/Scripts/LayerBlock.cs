using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LayerBlock : MainBehaviour
{
    public List<Layer> Layers { get; set; }

    public void Init()
    {
        Layers = new List<Layer>();
        foreach (Transform layer in transform)
        {
            Layers.Add(layer.GetComponent<Layer>());
        }
    }

    public void OnChangeLayer()
    {
        if (Layers[0] == null || Layers[1] == null)
        {
            Layers = new List<Layer>();
            foreach (Transform layer in transform)
            {
                Layers.Add(layer.GetComponent<Layer>());
            }
        }

        if (Layers[0].IsActive)
        {
            SwapLayer(Layers[0], Layers[1]);
        }
        else
        {
            SwapLayer(Layers[1], Layers[0]);
        }
    }

    public void SwapLayer(Layer toDeactive, Layer toActive, bool isInit = false)
    {
        toDeactive.IsActive = false;
        toActive.IsActive = true;

        toDeactive.SetTransparent(toActive: false);
        toActive.SetTransparent(toActive: true);

        UpDownTransition(toDeactive, toActive);
    }

    public void ClassicTransition(Layer toDeactive, Layer toActive)
    {
        Vector3 toDeactiveVector = new Vector3(transform.position.x + Constants.DEACTIVE_LAYER_OFFSET,
            transform.position.y + Constants.DEACTIVE_LAYER_OFFSET);
        Vector3 toActiveVector = new Vector3(transform.position.x + Constants.ACTIVE_LAYER_OFFSET,
            transform.position.y + Constants.ACTIVE_LAYER_OFFSET);

        toDeactive.transform.DOMove(toDeactiveVector, Constants.LAYER_MOVE_DURATION).SetEase(Ease.InCubic);
        toActive.transform.DOMove(toActiveVector, Constants.LAYER_MOVE_DURATION).SetEase(Ease.InCubic);
    }

    public void UpDownTransition(Layer toDeactive, Layer toActive)
    {
        Vector3 toDown = new Vector3(transform.position.x + Constants.DEACTIVE_LAYER_ALPHA, -2);
        Vector3 toUp = new Vector3(transform.position.x, transform.position.y);
        Vector3 toDeactiveVector = new Vector3(transform.position.x + Constants.DEACTIVE_LAYER_OFFSET,
            transform.position.y + Constants.DEACTIVE_LAYER_OFFSET);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(toActive.transform.DOMove(toDown, 0.1f)
            .SetEase(Ease.Linear));
        mySequence.Append(toActive.transform.DOMove(toUp, 0.1f)
            .SetEase(Ease.Linear));
        mySequence.Append(toDeactive.transform.DOMove(toDeactiveVector, Constants.LAYER_MOVE_DURATION)
            .SetEase(Ease.Linear));
    }
}