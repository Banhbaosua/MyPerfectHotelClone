using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ObjectPool<T> : ScriptableObject where T : MonoBehaviour
{
    [SerializeField] string poolName;
    [SerializeField] T prefab;
    [SerializeField] int amount;
    private Queue<T> pool;
    protected CompositeDisposable disposables = new CompositeDisposable();
    public void Initiate(Transform poolHolder = null)
    {
        var poolParent = new GameObject(poolName);
        pool = new Queue<T>();
        for(int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(prefab.gameObject,poolParent.transform);
            var finalGo = PoolObjModify(go);
            go.SetActive(false);
            pool.Enqueue(finalGo.GetComponent<T>());
        }
        poolParent.transform.SetParent(poolHolder);
    }

    public T Borrow()
    {
        return pool.Dequeue();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj.GetComponent<T>());
    }

    public virtual void ResetObject(){ }
    public abstract GameObject PoolObjModify(GameObject obj);

    private void OnDisable()
    {
        disposables?.Clear();
    }
}
