using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    public virtual void ReleaseObject()
    {
        if (gameObject == null) return;
        transform.SetParent(ObjectPool.Instance != null ? ObjectPool.Instance.transform : null);
        Pool.Release(gameObject);
    }
}