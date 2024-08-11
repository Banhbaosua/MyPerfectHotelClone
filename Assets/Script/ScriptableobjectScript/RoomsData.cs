using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomsData",menuName ="Data/RoomsData")]
public class RoomsData : ScriptableObject
{
    [SerializeField] List<Room> list;
    public List<Room> List => list;
    public void AddRoom(Room room)
    {
        list.Add(room);
    }
}
