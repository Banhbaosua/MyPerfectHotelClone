using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
[CreateAssetMenu(fileName ="CashPool",menuName ="Pools/CashPool")]
public class CashPool : ObjectPool<Cash>
{
    public override GameObject PoolObjModify(GameObject obj)
    {
        return obj;
    }
}
