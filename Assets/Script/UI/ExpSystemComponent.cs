using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSystemComponent : MonoBehaviour
{
    [SerializeField] ExpSystem expSystem;

    public void IncreaseXP() => expSystem.IncreaseXP(10f);
    public void ResetValue() => expSystem.ResetValue();
}
