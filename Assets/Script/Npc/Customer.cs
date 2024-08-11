using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    private bool hasRoom;
    private bool needToilet;
    private bool hasSlept;
    private Room asignedRoom;
    private float cash;
    private float tip;

    public bool HasRoom => hasRoom;
    public bool HasSlept => hasSlept;
    public bool NeedToilet => needToilet;
    public Room AsignedRoom => asignedRoom;
    public float Cash => cash;
    public float Tip => tip;
    public Customer(float cash,float tip)
    {
        hasRoom = false;
        needToilet = false;
        hasSlept = false;
        this.cash = cash;
        this.tip = tip;
    }

    public void SetRoom(Room room)
    {
        hasRoom = true;
        asignedRoom = room;
    }

    public void Slept()
    {
        hasSlept=true;
    }
}
