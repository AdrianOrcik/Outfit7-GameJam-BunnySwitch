using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LayerManager : MainBehaviour
{
    public LayerBlock[] LayerBlocksGO;
    public List<LayerBlock> LayerBlocks { get; set; }

    private void Start()
    {
        InputManager.instance.OnSwipe += OnChangeLayer;
        LayerBlocks = new List<LayerBlock>();
        for (int i = 0; i < 10; i++)
        {
            SpawnLayerBlock();
        }
    }

    public void SpawnLayerBlock()
    {
        int blockID = Random.Range(0, LayerBlocksGO.Length);

        LayerBlock layerBlock = Instantiate(LayerBlocksGO[blockID],
            new Vector3(Constants.LAYER_WIDTH * LayerBlocks.Count, 0, LayerBlocksGO[blockID].transform.position.z),
            Quaternion.identity);
        layerBlock.transform.SetParent(transform, false);
        layerBlock.Init();
        LayerBlocks.Add(layerBlock);
    }

    public void OnChangeLayer()
    {
        foreach (LayerBlock layerBlock in LayerBlocks)
        {
            layerBlock.OnChangeLayer();
        }
    }
}