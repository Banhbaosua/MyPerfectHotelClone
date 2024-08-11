using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : ScriptableObject
{
    [SerializeField] string objName;
    [SerializeField] GameObject prefab;
    [SerializeField] int amount;
    private Queue<GameObject> pool;
    public void InitiatePool(Transform poolParent = null)
    {
        var poolHolder = new GameObject(objName);
        pool = new Queue<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(prefab,poolHolder.transform);
            var finalGo = PoolObjModify(go);
            pool.Enqueue(finalGo);
        }
    }

    public GameObject SpawnObject()
    {
        return pool.Dequeue();
    }

    public void ReturnObject(GameObject obj)
    {
        pool.Enqueue(obj);
    }

    public abstract GameObject PoolObjModify(GameObject obj);
}
