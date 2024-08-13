using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SleepingRoom : Room
{
    [SerializeField] Transform bed;
    [SerializeField] Transform sleepPos;
    [SerializeField] List<CleanWork> cleanWorks;
    private bool used;
    public bool Used => used;
    Subject<Unit> onRoomUsed;
    public IObservable<Unit> OnRoomUsed => onRoomUsed;
    public Transform Bed => bed;
    public Transform SleepPos => sleepPos;

    protected override void Initiate()
    {
        base.Initiate();
        onRoomUsed = new Subject<Unit>();
    }

    private void Start()
    {
        foreach (CleanWork work in cleanWorks)
        {
            OnRoomUsed.Subscribe(_ => work.Available(true));
            work.OnWorkDone.Subscribe(_ =>
            {
                Debug.Log("done");
                isAvailable = CheckAvailableAfterWorkDone(); 
            });
        }
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
            return true;
        else return false;
    }
}
