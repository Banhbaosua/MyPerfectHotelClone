using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class SleepingRoom : Room, IUpgradable, ILoadSavable
{
    [SerializeField] Transform bed;
    [SerializeField] Transform sleepPos;
    [SerializeField] Transform currentRoomType;
    [SerializeField] Transform unlockZoneGO;
    [SerializeField] List<CleanWork> cleanWorks;
    [SerializeField] List<RoomTierData> roomTiers;
    [SerializeField] List<RoomTierType> roomTypes;
    [SerializeField] TextMeshProUGUI cashRequiredText;
    [SerializeField] Collider unlockCol;
    [SerializeField] ExpSystem expSystem;
    private bool canUpgrade;
    private bool used;
    private int currentTier;
    public bool CanUpgrade => canUpgrade;
    public bool Used => used;
    public int CurrentTier => currentTier;
    public int CurrentDeposit => currentDeposit;
    public int RentCost => roomTiers[currentTier - 1].CashRent;

    private Dictionary<RoomTierData, RoomTierType> roomTypeByRoomTier;
    public Dictionary<RoomTierData,RoomTierType> RoomTypeByRoomTier => roomTypeByRoomTier;
    protected override int CashRequired => roomTiers[currentTier-1].UpgradeCashRequired;
    Subject<Unit> onRoomUsed;
    Subject<RoomTierData> onRoomUpgrade;
    public IObservable<Unit> OnRoomUsed => onRoomUsed;
    public IObservable<RoomTierData> OnRoomUpgrade => onRoomUpgrade;
    
    public Transform Bed => bed;
    public Transform SleepPos => sleepPos;


    public RoomTierData CurrentRoomTierData => roomTiers[currentTier];

    public List<RoomTierData> TierData => roomTiers;

    protected override void Initiate()
    {
        base.Initiate();
        onRoomUsed = new Subject<Unit>();
        onRoomUpgrade = new Subject<RoomTierData>();
        roomTypeByRoomTier = new Dictionary<RoomTierData, RoomTierType>();
        Load();
    }

    private void Start()
    {
        foreach (CleanWork work in cleanWorks)
        {
            OnRoomUsed.Subscribe(_ => work.Available(true));
            work.OnWorkDone.Subscribe(_ =>
            {
                isAvailable = CheckAvailableAfterWorkDone(); 
            });
        }

        for(int i =0; i< roomTiers.Count; i++) 
        {
            roomTypeByRoomTier.Add(roomTiers[i], roomTypes[i]);
        }

        if (canUpgrade)
        {
            InitUpgradeStream();
        }
        else
        {
            unlockZoneGO.gameObject.SetActive(false);
        }
        currencySystem.OnCashChange.Subscribe(_ => cashRequiredText.text = (CashRequired - currentDeposit).ToString());
    }

    public void RoomUsed()
    {
        used = true;
        onRoomUsed.OnNext(Unit.Default);
    }

    bool CheckAvailableAfterWorkDone()
    {
        var notDoneWork = cleanWorks.Find(x => !x.IsDone);
        if (notDoneWork == null)
        {
            foreach(CleanWork work in cleanWorks)
            {
                work.WorkDone(false);
            }
            return true;
        }
        else return false;
    }

    void InitUpgradeStream()
    {
        unlockZoneGO.gameObject.SetActive(true);
        var checkUnlockStream = Observable.EveryFixedUpdate().Where(_ => currentDeposit >= CashRequired)
                .Subscribe(_ => Upgrade()).AddTo(disposables);

        unlockCol.OnTriggerEnterAsObservable()
                .Subscribe(_ =>
                {
                    StartCoroutine(DepositeCash());
                }).AddTo(disposables);

        unlockCol.OnTriggerExitAsObservable()
            .Subscribe(_ => StopCoroutine(DepositeCash())).AddTo(disposables);
    }
    public void Upgrade()
    {
        currentDeposit = 0;
        unlockZoneGO.gameObject.SetActive(false);
        currentRoomType.gameObject.SetActive(false);

        expSystem.IncreaseXP(roomTiers[currentTier-1].ExpWhenUnlock);

        onRoomUpgrade.OnNext(roomTiers[currentTier-1]);
        if (currentTier < roomTiers.Count+1)
            currentTier++;
        
        Save();
    }

    public override void Load()
    {
        var data = SaveGame.Load(this.gameObject.name, new SleepingRoomData(1, 0));
        currentDeposit = data.CurrentDeposit;
        currentTier = data.CurrentTier;
        Debug.Log(currentTier);
    }

    public override void Save()
    {
        SaveGame.Save(this.gameObject.name, new SleepingRoomData(CurrentTier, currentDeposit));
    }

    public void EnableUpdate()
    {
        canUpgrade = true;
        InitUpgradeStream();
    }
    public void DisableUpdate()
    {
        canUpgrade = false;
    }

    public void SetCurrentRoomType(Transform roomType)
    {
        currentRoomType = roomType;
    }
}
[Serializable]
public struct SleepingRoomData
{
    [SerializeField] public int CurrentTier;
    [SerializeField] public int CurrentDeposit;

    public SleepingRoomData(int currentTier, int currentDeposit)
    {
        this.CurrentTier = currentTier;
        this.CurrentDeposit = currentDeposit;
    }
}
[Serializable]
public class RoomTierType
{
    public Transform room1;
    public Transform room2;
}
