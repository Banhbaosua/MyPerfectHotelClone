using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ObjectPool : ScriptableObject
{
    [SerializeField] string poolName;
    [SerializeField] GameObject prefab;
    [SerializeField] int amount;
    private Queue<GameObject> pool;
    protected CompositeDisposable disposables = new CompositeDisposable();
    public void Initiate(Transform poolHolder = null)
    {
        var poolParent = new GameObject(poolName);
        pool = new Queue<GameObject>();
        for(int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(prefab,poolParent.transform);
            var finalGo = PoolObjModify(go);
            go.SetActive(false);
            pool.Enqueue(finalGo);
        }
        poolParent.transform.SetParent(poolHolder);
    }

    public GameObject Borrow()
    {
        return pool.Dequeue();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public abstract GameObject PoolObjModify(GameObject obj);

    private void OnDisable()
    {
        disposables?.Clear();
    }
}
