using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperWork : Work
{
    protected override void Initiate()
    {
        base.Initiate();
        isAvailable = true;
    }
}
