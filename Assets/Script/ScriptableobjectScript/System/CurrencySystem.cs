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

    public IObservable<int> OnCashCollect => cash;

    private void OnEnable()
    {
        Load();
        OnCashCollect.Subscribe(_ => Save());
    }
    public void Load()
    {
        CurrencyData? data = SaveGame.Load<CurrencyData>("CurrecySystemData");
        
        cash.Value = data != null ? data.Value.Cash : 0;
        token.Value = data != null ? data.Value.Token : 0;
        gem.Value = data != null ? data.Value.Gem : 0;
    }

    public void Save()
    {
        SaveGame.Save("CurrecySystemData", new CurrencyData(cash.Value, token.Value, gem.Value));
    }

    public void CollectCash(Cash cash)
    {
        this.cash.Value += cash.Value;
    }
}

public readonly struct CurrencyData
{
    public int Cash { get; }
    public int Token { get; }
    public int Gem { get; }
    public CurrencyData(int cash, int token, int diamond )
    {
        this.Cash = cash;
        this.Token = token;
        this.Gem = diamond;
    }
}
