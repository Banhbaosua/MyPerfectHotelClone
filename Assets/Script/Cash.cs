using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour
{
    private int value;
    public int Value => value;
    public void SetValue(int value)
    {
        this.value = value;
    }

    public void ResetValue()
    {

    }
}
