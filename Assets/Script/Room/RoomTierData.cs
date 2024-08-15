using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="RoomTierData",menuName ="Data/SleepingRoomTier")]
public class RoomTierData : ScriptableObject
{
    [SerializeField] int cashRent;
    [SerializeField] int expWhenUnlock;
    [SerializeField] int upgradeCashRequired;
    [SerializeField] Sprite img_Room1;
    [SerializeField] Sprite img_Room2;
    public int CashRent => cashRent;
    public int ExpWhenUnlock => expWhenUnlock;
    public int UpgradeCashRequired => upgradeCashRequired;
    public Sprite Img_Room1 => img_Room1;
    public Sprite Img_Room2 => img_Room2;

}
