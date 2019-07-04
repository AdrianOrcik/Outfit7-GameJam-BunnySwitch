using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum LayerTransform
{
    classic,
    upDown
}

public class LayerManager : MainBehaviour
{
    public LayerTransform LayerTransform = LayerTransform.classic;
    public Layer[] Layers = new Layer[2];

    private void Start()
    {
        InputManager.OnSwipe += OnChangeLayer;
        SwapLayer(Layers[1], Layers[0]);
    }

    public void OnChangeLayer()
    {
        if (Layers[0].IsActive)
        {
            SwapLayer(Layers[0], Layers[1]);
        }
        else
        {
            SwapLayer(Layers[1], Layers[0]);
        }
    }

    public void SwapLayer(Layer toDeactive, Layer toActive)
    {
        toDeactive.IsActive = false;
        toActive.IsActive = true;

        toDeactive.SetTransparent(toDeactive: true);
        toActive.SetTransparent(toDeactive: false);

        switch (LayerTransform)
        {
            case LayerTransform.classic:
                ClassicTransition(toDeactive, toActive);
                break;
            case LayerTransform.upDown:
                UpDownTransition(toDeactive, toActive);
                break;
        }
    }

    public void ClassicTransition(Layer toDeactive, Layer toActive)
    {
        Vector3 toDeactiveVector = new Vector3(Constants.DEACTIVE_LAYER_OFFSET, Constants.DEACTIVE_LAYER_OFFSET);
        Vector3 toActiveVector = new Vector3(Constants.ACTIVE_LAYER_OFFSET, Constants.ACTIVE_LAYER_OFFSET);

        toDeactive.transform.DOMove(toDeactiveVector, Constants.LAYER_MOVE_DURATION).SetEase(Ease.InCubic);
        toActive.transform.DOMove(toActiveVector, Constants.LAYER_MOVE_DURATION).SetEase(Ease.InCubic);
    }

    public void UpDownTransition(Layer toDeactive, Layer toActive)
    {
//        Vector3 toDeactiveVector = new Vector3(Constants.DEACTIVE_LAYER_ALPHA, Constants.DEACTIVE_LAYER_ALPHA);
//        Vector3 toActiveVector = new Vector3(Constants.ACTIVE_LAYER_OFFSET, Constants.ACTIVE_LAYER_OFFSET);

        Vector3 toDown = new Vector3(Constants.DEACTIVE_LAYER_ALPHA, -2);
        Vector3 toUp = new Vector3(0, 0);
        Vector3 toDeactiveVector = new Vector3(Constants.DEACTIVE_LAYER_OFFSET, Constants.DEACTIVE_LAYER_OFFSET);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(toActive.transform.DOMove(toDown, 0.1f)
            .SetEase(Ease.Linear));
        mySequence.Append(toActive.transform.DOMove(toUp, 0.1f)
            .SetEase(Ease.Linear));
        mySequence.Append(toDeactive.transform.DOMove(toDeactiveVector, Constants.LAYER_MOVE_DURATION)
            .SetEase(Ease.Linear));

//        toDeactive.transform.DOMove(toDeactiveVector, Constants.LAYER_MOVE_DURATION).SetEase(Ease.InCubic);
//        toActive.transform.DOMove(toActiveVector, Constants.LAYER_MOVE_DURATION).SetEase(Ease.InCubic);
    }
}