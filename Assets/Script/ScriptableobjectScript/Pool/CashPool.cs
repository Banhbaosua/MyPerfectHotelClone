using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="CashPool",menuName ="Pools/CashPool")]
public class CashPool : ObjectPool
{
    public override GameObject PoolObjModify(GameObject obj)
    {
        return obj;
    }
}
