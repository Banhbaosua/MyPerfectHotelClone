using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CustomerPool",menuName = "Pools/CustomerPool")]
public class CustomerPool : ObjectPool
{
    public override GameObject PoolObjModify(GameObject obj)
    {
        return obj;
    }
}
