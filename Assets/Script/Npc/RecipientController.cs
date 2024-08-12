using MonsterLove.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
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
    [SerializeField] AsignRoomWork asignWork;
    Queue<CustomerController> customerQueue;
    private StateMachine<RecipientState, RecipientDriver> sfm;
    public WaitingSlot WaitSlotAvailable => waitSlots.Where(x => x.IsAvailable == true).FirstOrDefault();
    // Start is called before the first frame update
    private void Awake()
    {
        sfm = new StateMachine<RecipientState, RecipientDriver>(this);
        customerQueue = new Queue<CustomerController>();
    }

    private void Start()
    {
        var getRoomStream = Observable.EveryFixedUpdate()
            .Where(_ => customerQueue.Count > 0 )
            .Where(_ => !asignWork.IsDone && asignWork.Room == null && customerQueue.Peek().IsWaiting)
            .Select(x => GetAvailableSleepingRoom());
            
        var CheckIfAnyEmptyRoom = getRoomStream.Where(x => x != null)
            .Subscribe(x =>
            {
                asignWork.SetRoom(x);
                asignWork.Available(true);
            });

        var CheckIfFull = getRoomStream.Where(x => x == null)
            .Subscribe(_ =>
            {
                asignWork.Available(false);
            });

        var asignRoomStream = Observable.EveryFixedUpdate()
            .Where(_ => asignWork.IsDone && asignWork.Room != null)
            .Subscribe(_ =>
            {
                RegisterRoom();
            });
    }
    // Update is called once per frame
    void Update()
    {
        sfm.Driver.Update.Invoke();
    }

    public SleepingRoom GetAvailableSleepingRoom()
    {
        var room = roomsData.List.Find(x => x.GetType() == typeof(SleepingRoom) && x.IsAvailable == true) as SleepingRoom;
        return room;
    }

    public void AddWaiting(CustomerController customer)
    {
        customerQueue.Enqueue(customer);
    }

    public void RegisterRoom()
    {
        var room = asignWork.Room;
        asignWork.Room.Occupied();
        customerQueue.Peek().AsignRoom(room);
        customerQueue.Dequeue();

        MoveQueueUp();
        asignWork.SetRoom(null);
        asignWork.Available(false);
        asignWork.WorkDone(false);
    }

    void MoveQueueUp()
    {
        int index = 0;
        foreach(var customer in customerQueue)
        {
            customer.SetDest(waitSlots[index].transform.position);
            index++;
        }
        for(int i = 0; i< waitSlots.Count - customerQueue.Count; i++) 
        {
            waitSlots[customerQueue.Count + i ].Available();
        }
    }
}