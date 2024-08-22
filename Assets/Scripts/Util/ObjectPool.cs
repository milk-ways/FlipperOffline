using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPool : DontDestroySingleton<ObjectPool>
{
    public List<ObjectInfo> objects;
    //string to objectpool
    private Dictionary<string, IObjectPool<GameObject>> poolDict = new();
    private Dictionary<string, GameObject> objDict = new();

    private string objname;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < objects.Count; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledObject, true, objects[i].defaultSize, objects[i].maxSize);

            objname = objects[i].objName;
            poolDict.Add(objname, pool);
            objDict.Add(objname, objects[i].prefab);

            for (int j = 0; j < objects[i].startCnt; j++)
            {
                Poolable pl = CreatePooledItem().GetComponent<Poolable>();
                if (!pl)
                {
                    //Debug.LogWarning($"You registered unpoolable object {objname} to object pool!");
                    return;
                }
                pl.transform.SetParent(transform);
                pl.ReleaseObject();
            }
        }
    }

    private GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(objDict[objname]);
        go.GetComponent<Poolable>().Pool = poolDict[objname];
        return go;
    }

    private void OnTakeFromPool(GameObject pooledItem)
    {
        pooledItem.SetActive(true);
    }

    private void OnReturnedToPool(GameObject pooledItem)
    {
        pooledItem.SetActive(false);
    }

    private void OnDestroyPooledObject(GameObject pooledItem)
    {
        Destroy(pooledItem);
    }

    public GameObject GetGO(string GOName)
    {
        if (!poolDict.ContainsKey(GOName))
        {
            //Debug.LogWarning($"{GOName} is not registered key!");
            return null;
        }

        objname = GOName;
        GameObject goObject = poolDict[GOName].Get();
        //goObject.transform.SetParent(null);
        return goObject;
    }
}

[System.Serializable]
public class ObjectInfo
{
    public string objName;
    public GameObject prefab;
    public int startCnt;
    public int defaultSize;
    public int maxSize;
}
