using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToiletRoom : Room
{
    [SerializeField] List<Toilet> toilets;
    [SerializeField] List<ToiletWaitSpot> waitSpots;

    public override bool IsAvailable => GetEmptyToilet() != null;

    public Toilet GetEmptyToilet()
    {
        return toilets.Where(x => x.IsAvailable).FirstOrDefault();
    }

    public Transform GetEmptyWaitSpot()
    {

        return null;
    }
    private void Start()
    {
        Debug.Log(IsAvailable);
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

