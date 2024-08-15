using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencySystem",menuName ="Systems/CashSystem")]
public class CurrencySystem : ScriptableObject, ILoadSavable
{
    [SerializeField] ReactiveProperty<int> cash;
    [SerializeField] ReactiveProperty<int> token;
    [SerializeField] ReactiveProperty<int> gem;

    public int Cash => cash.Value;
    public IObservable<int> OnCashChange => cash;

    public void Initiate() 
    {
        Load();
        OnCashChange.Subscribe(_ =>
        {
            Save(); 
        });
    }
    public void Load()
    {
        CurrencyData data = SaveGame.Load("CurrecyData", new CurrencyData(0,0,0));
        cash.Value = data.Cash;
        token.Value = data.Token;
        gem.Value = data.Gem;

    }

    public void Save()
    {
        SaveGame.Save("CurrecyData", new CurrencyData(cash.Value, token.Value, gem.Value));
    }

    public void CollectCash(Cash cash)
    {
        this.cash.Value += cash.Value;
    }

    public int RequestCash(int value)
    {
        if (Cash - value > 0)
        {
            cash.Value -= value;
            return value;
        }
        else
        {
            int tmpCash = Cash;
            cash.Value = 0;
            return tmpCash;
        }
    }
}

[Serializable]
public class CurrencyData
{
    [SerializeField] public int Cash;
    [SerializeField] public int Token;
    [SerializeField] public int Gem;
    public CurrencyData(int cash, int token, int diamond )
    {
        this.Cash = cash;
        this.Token = token;
        this.Gem = diamond;
    }
}
