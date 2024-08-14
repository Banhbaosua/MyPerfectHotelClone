using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnlockable
{
    public bool IsUnlocked { get; }
    public void Unlock();

    public IEnumerator DepositeCash();
}
