using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToiletRoom : Room, IUnlockable
{
    [SerializeField] List<Toilet> toilets;
    [SerializeField] List<ToiletWaitSpot> waitSpots;
    private bool isUnlock;
    public override bool IsAvailable => GetEmptyToilet() != null && isUnlock;

    public bool IsUnlock { get => isUnlock; }

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
        isUnlock = true;
    }

    private void Start()
    {
        Unlock();
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

