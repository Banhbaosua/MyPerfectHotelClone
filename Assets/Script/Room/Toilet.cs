using System;
using UnityEngine;

[Serializable]
public class Toilet : MonoBehaviour
{
    [SerializeField] Transform sitSpot;
    [SerializeField] int useTimes;
    private bool isAvailable;
    public bool IsAvailable => isAvailable && useTimes > 0;
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
        useTimes--;
    }
}
