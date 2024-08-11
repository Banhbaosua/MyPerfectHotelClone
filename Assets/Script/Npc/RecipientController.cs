using MonsterLove.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum RecipientState
{
    AsignRoom,
    Waiting,
}

public class RecipientDriver
{
    public StateEvent Update;
}
public class RecipientController : MonoBehaviour
{
    [SerializeField] RoomsData roomsData;
    [SerializeField] List<WaitingSlot> waitSlots;
    Queue<CustomerController> customers;
    private StateMachine<RecipientState, RecipientDriver> sfm;
    public WaitingSlot WaitSlotAvailable => waitSlots.FirstOrDefault(x=> x.IsAvailable == true);
    // Start is called before the first frame update
    private void Awake()
    {
        sfm = new StateMachine<RecipientState, RecipientDriver>(this);
        customers = new Queue<CustomerController>();
    }
    // Update is called once per frame
    void Update()
    {
        sfm.Driver.Update.Invoke();
    }

    public Room GetAvailableRoom()
    {
        var room = roomsData.List.Find(x => x.GetType() == typeof(Room) && x.IsAvailable == true);
        return room;
    }

    public void AddWaiting(CustomerController customer)
    {
        customers.Enqueue(customer);
    }

    public void RemoveWaiting()
    { 
        customers.Dequeue();
        waitSlots[waitSlots.Count - 1].Available();
    }
}