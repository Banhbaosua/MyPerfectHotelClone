using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGameFree;
using UniRx;
using System;

[CreateAssetMenu(fileName ="ExpSystem",menuName ="Systems/ExpSystem")]
public class ExpSystem : ScriptableObject, ILoadSavable
{
    [SerializeField] ReactiveProperty<float> exp;
    [SerializeField] float growthRate;

    private const float BASEREQUIREDXP = 100f;
    private int _currentLevel = 0;

    private Subject<Unit> onLevelUp;
    public IObservable<Unit> OnLevelUp => onLevelUp;
    public IObservable<float> OnExpReceive => exp;

    public int CurrentLevel => _currentLevel;
    public float CurrentXp => exp.Value;

    CompositeDisposable disposables;
    public float GetExpForLevel(int level)
    {
        return BASEREQUIREDXP * Mathf.Pow(growthRate,level-1);
    }
    
    public void LevelUp(float overloadXp)
    {
        _currentLevel++;
        exp.Value = overloadXp;
    }

    public void Initiate()
    {
        disposables = new CompositeDisposable();
        onLevelUp = new Subject<Unit>();
        //levelUp stream
        OnExpReceive.Where(x => x >= GetExpForLevel(_currentLevel + 1))
            .Subscribe(x =>
            {
                LevelUp(x - GetExpForLevel(_currentLevel + 1));
                onLevelUp.OnNext(Unit.Default);
            }).AddTo(disposables);

        OnExpReceive.Subscribe(_ => Save());
    }
    public void Load()
    {
        var data = SaveGame.Load<ExpSystemData>("ExpSystemData");
        exp.Value = data.exp;
        _currentLevel = data.currentLevel;
    }

    public void Save()
    {
        SaveGame.Save("ExpSystemData",new ExpSystemData(exp.Value,_currentLevel));
    }

    private void OnDisable()
    {
        disposables?.Clear();
    }
}
[Serializable]
public struct ExpSystemData
{
    public float exp;
    public int currentLevel;
    public ExpSystemData(float exp, int currentLevel)
    {
        this.exp = exp;
        this.currentLevel = currentLevel;
    }
}
