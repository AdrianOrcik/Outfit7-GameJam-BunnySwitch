using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCloudBehaviour : MonoBehaviour
{
    public List<GameObject> Clouds { get; set; }

    public void Init()
    {
        Clouds = new List<GameObject>();
        foreach (Transform cloud in transform)
        {
            Clouds.Add(cloud.gameObject);
        }

        GenerateClouds();
    }

    private void GenerateClouds()
    {
        // ReSharper disable once PossibleLossOfFraction
        float cloudSize = Random.Range(10, 30) / 10;
        for (int i = 0; i < Clouds.Count; i++)
        {
            if (Random.Range(0, 50) > 20)
            {
                Clouds[i].transform.localScale = new Vector3(cloudSize, cloudSize, 1);
            }
        }
    }

    void Update()
    {
        MoveLeft();
    }

    public void MoveLeft()
    {
        if (MainModel.GameManager.IsPlaying)
        {
            transform.Translate(Vector3.left * Time.deltaTime * Constants.PARALLAX_CLOUD_SPEED);
        }
    }
}