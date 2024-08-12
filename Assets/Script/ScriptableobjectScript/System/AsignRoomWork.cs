using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsignRoomWork : Work
{
    private Room room;
    public Room Room => room;
    public override void WorkDone(bool v)
    {
        base.WorkDone(v);
    }

    public void SetRoom(Room room)
    {
        this.room = room;
    }
}
