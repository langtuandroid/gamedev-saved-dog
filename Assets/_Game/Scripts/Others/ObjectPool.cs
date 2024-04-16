using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<Pool> poolList = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

   [Inject] private DiContainer _diContainer;

   private void Awake()
    {
        foreach (Pool pool in poolList)
        {
            Queue<GameObject> q = new Queue<GameObject>();

            for (int i = 0; i < pool.countToSpawn; i++)
            {
                GameObject obj = _diContainer.InstantiatePrefab(pool.poolPrefab);
                obj.SetActive(false);

                q.Enqueue(obj);
            }

            objectPools.Add(pool.objectTag, q);
        }
    }

    public GameObject GetFromPool(string tag)
    {
        Pool tempPool = new Pool();
        foreach (Pool pool in poolList)
        {
            if (tag == pool.objectTag)
            {
                tempPool = pool;
            }
        }

        if (objectPools[tag].Count > 0)
        {
            return objectPools[tag].Dequeue();
        }

        if (!tempPool.isCanGrow)
        {
            return null;
        }

        GameObject obj = _diContainer.InstantiatePrefab(tempPool.poolPrefab);
        return obj;

    }
    public void ReturnToPool(string tag, GameObject obj)
    {
        objectPools[tag].Enqueue(obj);
        obj.SetActive(false);
    }
}

[System.Serializable]
public class Pool
{
    [FormerlySerializedAs("poolObjectPrefab")]
    public GameObject poolPrefab;
    [FormerlySerializedAs("poolCount")]
    public int countToSpawn;
    [FormerlySerializedAs("canGrow")]
    public bool isCanGrow;
    [FormerlySerializedAs("tag")]
    public string objectTag;
}
