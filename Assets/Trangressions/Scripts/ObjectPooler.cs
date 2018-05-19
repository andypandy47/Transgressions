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
        if (!sharedInstance)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
            
    }

    private void Start()
    {
        RestartPool();
    }

    public void RestartPool()
    {
      //  if (GameController.gc.firstRun)
       // {
            pooledObjects.Clear();
            splatterParent = transform.GetChild(0).gameObject;

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
                GameObject obj = (GameObject)Instantiate(splatter, splatterParent.transform);
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
                DontDestroyOnLoad(splatterPool[i].gameObject);
            }
            print("Pool restart");

       // }

        
    }

    public GameObject GetPooledObject(string tag)
    {

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
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

                return splatterPool[currentObj];
            }
        }
        return null;
    }

    public void DeactivatePooledObjects()
    {
        for (int i = 0; i < splatterPool.Count; i++)
        {
            //DontDestroyOnLoad(splatterPool[i].gameObject);
            splatterPool[i].SetActive(false);
        }

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].transform.parent = null;
            DontDestroyOnLoad(pooledObjects[i].gameObject);
            pooledObjects[i].SetActive(false);
        }
    }
}
