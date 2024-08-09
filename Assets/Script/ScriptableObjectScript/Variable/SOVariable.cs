using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOVariable<T> : ScriptableObject
{
    [SerializeField] T baseValue;
    public T value;

    private void OnEnable()
    {
        value = baseValue;
    }
}
