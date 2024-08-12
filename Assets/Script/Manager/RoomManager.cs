using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] RoomsData roomsData;
    private void Awake()
    {
        InitiateRoomsData();
    }

    // Update is called once per frame
    void InitiateRoomsData()
    {
        roomsData.Init();
        foreach(Room room in this.transform.GetComponentsInChildren<Room>())
        {
            roomsData.AddRoom(room);
        }
    }
}
