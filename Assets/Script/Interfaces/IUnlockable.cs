using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnlockable
{
    public bool IsUnlock { get; }
    public void Unlock();
}
