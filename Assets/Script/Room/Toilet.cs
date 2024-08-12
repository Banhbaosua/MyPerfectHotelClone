using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Toilet : MonoBehaviour
{
    [SerializeField] Transform sitSpot;
    private bool isAvailable;
    public bool IsAvailable => isAvailable;
    public Transform SitSpot => sitSpot;
    private void Awake()
    {
        Available();
    }
    public void Available()
    {
        isAvailable = true;
    }

    public void Occupied()
    {
        isAvailable = false;
    }
}
