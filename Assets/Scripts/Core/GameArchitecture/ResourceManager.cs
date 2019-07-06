using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using Object = UnityEngine.Object;


[Serializable]
public class PoolObj
{
    public PoolType PoolType;
    public int Amount;
    public GameObject Obj;
}

[Serializable]
public enum PoolType
{
    Layerblock0 = 0,
    Layerblock1 = 1,
    Layerblock2 = 2,
    Layerblock3 = 3,
    Layerblock4 = 4,
    Layerblock5 = 5,
    Layerblock6 = 6
}

public class ResourceManager : MainBehaviour
{
    public PoolObj[] GameBlocks = new PoolObj[5];
    public Dictionary<PoolType, Queue<MonoBehaviour>> PoolDictionary;

    private void Awake()
    {
        MainModel.ResourceManager = this;
        AssignClass(this);
        PoolDictionary = new Dictionary<PoolType, Queue<MonoBehaviour>>();
    }

    public void LoadResources()
    {
        LoadPoolers(PoolType.Layerblock0);
        LoadPoolers(PoolType.Layerblock1);
        LoadPoolers(PoolType.Layerblock2);
        LoadPoolers(PoolType.Layerblock3);
    }

    /// <summary>
    /// Load objects into pooler depends from PoolDefinitions
    /// </summary>
    private void LoadPoolers(PoolType poolType)
    {
        foreach (PoolObj obj in GameBlocks)
        {
            if (obj.PoolType == poolType)
            {
                Queue<MonoBehaviour> objPool = new Queue<MonoBehaviour>();
                for (int i = 0; i < obj.Amount; i++)
                {
                    MonoBehaviour prefab = LoadPool(obj.Obj);
                    prefab.gameObject.SetActive(false);
                    prefab.transform.SetParent(transform);
                    objPool.Enqueue(prefab);
                }

                PoolDictionary.Add(poolType, objPool);
            }
        }
    }

    /// <summary>
    /// Method return monoBehaviour object by objID
    /// </summary>
    public MonoBehaviour SpawnFromPool(PoolType poolType, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(poolType)) return null;

        MonoBehaviour objectToSpawn = PoolDictionary[poolType].Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = new Vector3(0, 0, 0);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        PoolDictionary[poolType].Enqueue(objectToSpawn);

        return objectToSpawn;
    }


    public void DisablePool()
    {
        foreach (Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }

        PoolDictionary = new Dictionary<PoolType, Queue<MonoBehaviour>>();
        LoadResources();
    }

    private MonoBehaviour LoadPool(GameObject obj)
    {
        return Instantiate(obj, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity)
            .GetComponent<MonoBehaviour>();
    }
}