using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LayerManager : MainBehaviour
{
    public List<LayerBlock> LayerBlocks { get; set; }

    private void Start()
    {
        InputManager.instance.OnSwipe += OnChangeLayer;
        LayerBlocks = new List<LayerBlock>();
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                SpawnLayerBlock(0);
            }
            else
            {
                SpawnLayerBlock(Random.Range(1, ResourceManager.PoolDictionary.Count));
            }
        }

#if !UNITY_EDITOR && ( UNITY_ANDROID || UNITY_IOS )
          MainModel.GameManager.OnStartGame += OnChangeLayer;
#endif
        MainModel.GameManager.OnStartGame += OnChangeLayer;
    }

    public void SpawnNextBlock()
    {
        SpawnLayerBlock(Random.Range(1, ResourceManager.PoolDictionary.Count));
    }

    public void SpawnLayerBlock(int blockID)
    {
        LayerBlock layerBlock = (LayerBlock) ResourceManager.SpawnFromPool((PoolType) blockID,
            new Vector3(Constants.LAYER_WIDTH * LayerBlocks.Count, 0, 0),
            Quaternion.identity);

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