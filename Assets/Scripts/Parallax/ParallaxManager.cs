using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: refactor spawn system
//TODO: figureOut transform reaction dependence from player
public class ParallaxManager : MonoBehaviour
{
    public ParallaxLayerBehaviour FrontLayerGO;
    public ParallaxLayerBehaviour BackLayersGO;
    public ParallaxCloudBehaviour CloudsLayersGO;

    public List<ParallaxLayerBehaviour> FrontLayers { get; set; }
    public List<ParallaxLayerBehaviour> BackLayers { get; set; }
    public List<ParallaxCloudBehaviour> CloudsLayers { get; set; }

    void Start()
    {
        FrontLayers = new List<ParallaxLayerBehaviour>();
        BackLayers = new List<ParallaxLayerBehaviour>();
        CloudsLayers = new List<ParallaxCloudBehaviour>();

        for (int i = 0; i < 10; i++)
        {
            GenerateBackground();
        }
    }

    void GenerateBackground()
    {
        GenerateFrontLayer();
        GenerateFrontLayer();

        GenerateBackLayer();
        GenerateBackLayer();

        GenerateClouds();
        GenerateClouds();
    }

    void GenerateClouds()
    {
        Vector3 cloudPosition = new Vector3(
            transform.position.x + (Constants.PARALLAX_BACKGROUD_WIDTH * CloudsLayers.Count),
            5, transform.position.z);
        ParallaxCloudBehaviour cloudLayer = Instantiate(CloudsLayersGO, cloudPosition, Quaternion.identity);
        cloudLayer.Init();
        CloudsLayers.Add(cloudLayer);
        cloudLayer.transform.SetParent(transform, true);
    }

    void GenerateBackLayer()
    {
        Vector3 backLayerPosition = new Vector3(
            transform.position.x + (Constants.PARALLAX_BACKGROUD_WIDTH * BackLayers.Count),
            0.8f, transform.position.z);
        ParallaxLayerBehaviour backLayer = Instantiate(BackLayersGO, backLayerPosition, Quaternion.identity);
        BackLayers.Add(backLayer);
        backLayer.transform.SetParent(transform, true);
    }

    void GenerateFrontLayer()
    {
        Vector3 frontLayerPosition = new Vector3(
            transform.position.x + (Constants.PARALLAX_BACKGROUD_WIDTH * FrontLayers.Count),
            0f, transform.position.z);
        ParallaxLayerBehaviour frontLayer = Instantiate(FrontLayerGO, frontLayerPosition, Quaternion.identity);
        FrontLayers.Add(frontLayer);
        frontLayer.transform.SetParent(transform, true);
    }
}