using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ExpSystem expSystem;
    [SerializeField] List<ObjectPool> pools;
    private void Awake()
    {
        expSystem.Initiate();
        foreach(var pool in pools) 
        {
            pool.Initiate(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
