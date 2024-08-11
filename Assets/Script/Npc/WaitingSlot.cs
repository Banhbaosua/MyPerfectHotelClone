using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingSlot : MonoBehaviour
{
    private bool isAvailable;
    public bool IsAvailable => isAvailable;

    private void Awake()
    {
        isAvailable = true;
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
