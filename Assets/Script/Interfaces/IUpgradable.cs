using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    public List<RoomTierData> TierData { get; }
    public int CurrentTier { get; }
    public int CurrentDeposit { get; }
    public bool CanUpgrade { get; }
    public void Upgrade();
    public void EnableUpdate();
}
