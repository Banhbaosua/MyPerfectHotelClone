using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ToiletRoom : Room, IUnlockable,ILoadSavable
{
    [SerializeField] int toiletIndex;
    [SerializeField] List<Toilet> toilets;
    [SerializeField] List<ToiletWaitSpot> waitSpots;
    [SerializeField] Collider unlockCol;
    [SerializeField] int cashUnlockRequired;
    [SerializeField] Transform lockedWall;
    [SerializeField] Transform toiletRoom;

    private bool isUnlocked;
    public override bool IsAvailable => GetEmptyToilet() != null && isUnlocked;

    public bool IsUnlocked => isUnlocked;
    protected override int CashRequired => cashUnlockRequired;

    protected override void Awake()
    {
        base.Awake();
        Load();
    }

    private void Start()
    {
        if (!isUnlocked)
        {
            lockedWall.gameObject.SetActive(true);
            toiletRoom.gameObject.SetActive(false);

            var checkUnlockStream = Observable.EveryFixedUpdate().Where(_ => currentDeposit >= cashUnlockRequired)
                .Subscribe(_ => Unlock()).AddTo(disposables);

            unlockCol.OnTriggerEnterAsObservable()
                .Subscribe(_ =>
                {
                    StartCoroutine(DepositeCash());
                }).AddTo(disposables);

            unlockCol.OnTriggerExitAsObservable()
                .Subscribe(_ => StopCoroutine(DepositeCash())).AddTo(disposables);

            cashRequiredText.text = (CashRequired - currentDeposit).ToString();
        }
        else
        {
            lockedWall.gameObject.SetActive(false);
            toiletRoom.gameObject.SetActive(true);
        }
    }

    public Toilet GetEmptyToilet()
    {
        return toilets.Find(x => x.IsAvailable);
    }

    public Transform GetEmptyWaitSpot()
    {

        return null;
    }

    public void Unlock()
    {
        lockedWall.gameObject.SetActive(false);
        toiletRoom.gameObject.SetActive(true);
        isUnlocked = true;
        Save();
    }

    public override void Load()
    {
        var data = SaveGame.Load<UnlockProgress>("Toilet" + toiletIndex.ToString(),new UnlockProgress(false,0));

        isUnlocked = data.IsUnlock;
        currentDeposit = data.CurrentDeposit;
    }

    public override void Save()
    {
        SaveGame.Save("Toilet" + toiletIndex.ToString(), new UnlockProgress(isUnlocked,currentDeposit));
    }
}
[Serializable]
public class ToiletWaitSpot
{
    [SerializeField] Transform pos;
    private bool isEmpty;
    public bool IsEmpty => isEmpty;
    public void Available()
    {
        isEmpty = true;
    }

    public void Occupied()
    {
        isEmpty = false;
    }
}

[Serializable]
public struct UnlockProgress
{
    [SerializeField] public bool IsUnlock;
    [SerializeField] public int CurrentDeposit;
    public UnlockProgress(bool isUnlock, int currentDeposit)
    {
        this.IsUnlock = isUnlock;
        this.CurrentDeposit = currentDeposit;
    }
}

