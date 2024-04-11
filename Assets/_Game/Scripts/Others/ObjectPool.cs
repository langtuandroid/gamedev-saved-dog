using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject poolObjectPrefab;
        public int poolCount;
        public bool canGrow;
        public string tag;
    }
    public List<Pool> poolList = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

   [Inject] private DiContainer _diContainer;

   private void Awake()
    {
        foreach (Pool pool in poolList)
        {
            Queue<GameObject> q = new Queue<GameObject>();

            for (int i = 0; i < pool.poolCount; i++)
            {
                GameObject obj = _diContainer.InstantiatePrefab(pool.poolObjectPrefab);
                obj.SetActive(false);

                q.Enqueue(obj);
            }

            objectPools.Add(pool.tag, q);
        }
    }

    public GameObject GetFromPool(string tag)
    {
        Pool tempPool = new Pool();
        foreach (Pool pool in poolList)
        {
            if (tag == pool.tag)
            {
                tempPool = pool;
            }
        }


        if (objectPools[tag].Count > 0)
        {
            return objectPools[tag].Dequeue();
        }

        if (!tempPool.canGrow)
        {
            return null;
        }

        GameObject obj = _diContainer.InstantiatePrefab(tempPool.poolObjectPrefab);
        return obj;

    }
    public void ReturnToPool(string tag, GameObject obj)
    {
        objectPools[tag].Enqueue(obj);
        obj.SetActive(false);
    }
}
