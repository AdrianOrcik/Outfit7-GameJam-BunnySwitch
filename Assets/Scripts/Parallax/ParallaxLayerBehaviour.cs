using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayerBehaviour : MonoBehaviour
{
    void Update()
    {
        MoveLeft();
    }

    public void MoveLeft()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            transform.Translate(Vector3.left * Time.deltaTime * Constants.PARALLAX_BACK_LAYER_SPEED);
        }
    }
}