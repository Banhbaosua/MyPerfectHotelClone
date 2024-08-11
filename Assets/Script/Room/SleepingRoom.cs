using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingRoom : Room
{
    [SerializeField] Transform bed;
    public Transform Bed => bed;
}
