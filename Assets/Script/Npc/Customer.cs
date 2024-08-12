using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    private bool hasRoom;
    private bool needToilet;
    private bool hasSlept;
    private bool done;
    private Room asignedRoom;
    private Toilet currentToilet;
    private float cash;
    private float tip;

    public bool HasRoom => hasRoom;
    public bool HasSlept => hasSlept;
    public bool NeedToilet => needToilet;
    public bool Done => done;
    public Room AsignedRoom => asignedRoom;
    public Toilet CurrentToilet => currentToilet;
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

    public T GetRoom<T>() where T : Room
    {
        T room = asignedRoom as T;
        return room;
    }
    public void SetRoom(Room room)
    {
        hasRoom = true;
        asignedRoom = room;
    }

    public void SetToilet(Toilet toilet) 
    {
        currentToilet = toilet;
    }

    public void Slept()
    {
        hasSlept=true;
    }

    public void RequestToilet(bool value)
    {
        needToilet=value;
    }

    public void Out()
    {
        done = true;
    }
}
