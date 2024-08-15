using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ExpSystem expSystem;
    [SerializeField] CashPool cashPool;
    [SerializeField] CustomerPool customerPool;
    [SerializeField] CurrencySystem currencySystem;
    private void Awake()
    {
        expSystem.Initiate();
        cashPool.Initiate(this.transform);
        customerPool.Initiate(this.transform);
        currencySystem.Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
