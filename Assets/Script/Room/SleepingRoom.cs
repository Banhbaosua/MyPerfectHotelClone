using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingRoom : Room
{
    [SerializeField] Transform bed;
    [SerializeField] Transform sleepPos;
    public Transform Bed => bed;
    public Transform SleepPos => sleepPos;
}
