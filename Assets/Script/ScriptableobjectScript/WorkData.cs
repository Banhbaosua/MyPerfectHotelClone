using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WorkData", menuName = "Data/WorkData")]
public class WorkData : ScriptableObject
{
    [SerializeField] float workTime;

    public float WorkTime => workTime;
}
