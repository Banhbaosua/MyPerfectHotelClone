using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ExpSystem expSystem;
    private void Awake()
    {
        expSystem.Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
