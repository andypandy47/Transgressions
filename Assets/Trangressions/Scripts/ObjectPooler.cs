using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
}

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler sharedInstance;

    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;
    public GameObject splatter;
    public int amountToSplatterPool;
    public List<GameObject> splatterPool;

    GameObject splatterParent;
    int currentObj = 0;

    public bool initialLoad = true;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    private void Start()
    {
        RestartPool();
    }

    public void RestartPool()
    {
        if (initialLoad)
        {
            pooledObjects.Clear();
            splatterParent = GameObject.FindGameObjectWithTag("SplatterParent");

            pooledObjects = new List<GameObject>();
            splatterPool = new List<GameObject>();

            foreach (ObjectPoolItem item in itemsToPool)
            {
                //print("For each object to pool");
                for (int i = 0; i < item.amountToPool; i++)
                {
                    //print("Pooled" + (i + 1));
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }

            //Specifically for splatter pooling
            for (int i = 0; i < amountToSplatterPool; i++)
            {
                GameObject obj = Instantiate(splatter, splatterParent.transform);
                obj.SetActive(false);
                splatterPool.Add(obj);
            }

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                //if (pooledObjects[i].)

                DontDestroyOnLoad(pooledObjects[i].gameObject);
            }

            for (int i = 0; i < splatterPool.Count; i++)
            {
                //DontDestroyOnLoad(splatterPool[i].gameObject);
            }

            initialLoad = false;
        }

        
    }

    public GameObject GetPooledObject(string tag)
    {

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[currentObj];
            }
        }
        return null;
    }

    public GameObject GetSplatter(string tag)
    {

        for (int i = 0; i < splatterPool.Count; i++)
        {
            if (!splatterPool[currentObj].activeInHierarchy && splatterPool[currentObj].tag == tag)
            {
                currentObj++;
                if (currentObj >= splatterPool.Count)
                    currentObj = 0;

                return splatterPool[currentObj];
            }
            else if (splatterPool[currentObj].activeInHierarchy && splatterPool[currentObj].tag == tag)
            {
                currentObj++;
                if (currentObj >= splatterPool.Count)
                    currentObj = 0;

                print("Switch to new position");
                return splatterPool[currentObj];

            }

        }

        return null;
    }
}
