using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
[CreateAssetMenu(fileName = "SpawnSystem",menuName = "Systems/SpawnSystem")]
public class SpawnSystem : ScriptableObject
{
    [SerializeField] List<ObjectPool> pools;

    public GameObject Spawn<T>( Transform location)
    { 
        var go = pools.Find(x => x.GetType() == typeof(T)).Borrow();
        go.transform.position = location.position;
        go.SetActive(true);

        return go;
    }
}
