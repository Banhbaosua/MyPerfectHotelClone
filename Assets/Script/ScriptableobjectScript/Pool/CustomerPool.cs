using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
[CreateAssetMenu(fileName = "CustomerPool",menuName = "Pools/CustomerPool")]
public class CustomerPool : ObjectPool
{
    public override GameObject PoolObjModify(GameObject obj)
    {
        obj.GetComponent<CustomerController>().OnGoOut.Select(_ => obj)
            .Subscribe(x => ReturnObject(x)).AddTo(disposables);
        return obj;
    }
}
