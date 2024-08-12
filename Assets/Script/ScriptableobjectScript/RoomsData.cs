using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void Init()
    {
        list = new List<Room>();
    }

    public T FindAvailableByType<T>() where T : Room
    {
        var room = list.Find(x => x.GetType() == typeof(T) && x.IsAvailable);

        return room as T;
    }

    public T FindFirst<T>() where T : Room
    {
        var room = list.Find(x => x.GetType() == typeof(T));

        return room as T;
    }    
}
