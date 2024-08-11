using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGameFree;
using UniRx;
using System;

[CreateAssetMenu(fileName = "ExpSystem", menuName = "Systems/ExpSystem")]
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
    public float NextLevelExpRequired => GetExpForLevel(CurrentLevel+1);

    CompositeDisposable disposables;
    public float GetExpForLevel(int level)
    {
        return BASEREQUIREDXP * Mathf.Pow(growthRate, level - 1);
    }

    public void LevelUp(float Xp)
    {
        exp.Value = Xp-NextLevelExpRequired;
        _currentLevel++;
    }

    public void Initiate()
    {
        disposables = new CompositeDisposable();
        onLevelUp = new Subject<Unit>();
        //levelUp stream
        OnExpReceive.Where(x => x >= NextLevelExpRequired)
            .Subscribe(x =>
            {
                LevelUp(x);
                onLevelUp.OnNext(Unit.Default);
            }).AddTo(disposables);

        OnExpReceive.Subscribe(_ => Save());
    }
    public void Load()
    {
        if (!SaveGame.Exists("ExpSystemData"))
        {
            exp.Value = 0;
            _currentLevel = 0;
            return;
        }
        var data = SaveGame.Load<ExpSystemData>("ExpSystemData");
        exp.Value = data.Exp;
        _currentLevel = data.CurrentLevel;
    }

    public void Save()
    {
        SaveGame.Save("ExpSystemData", new ExpSystemData(exp.Value, _currentLevel));
    }

    private void OnDisable()
    {
        disposables?.Clear();
    }

    public void IncreaseXP(float xp)
    {
        exp.Value += xp;
    }

    public void ResetValue()
    {
        exp.Value = 0;
        _currentLevel =0;
    }
}
[Serializable]
public readonly struct ExpSystemData
{
    public float Exp { get; }
    public int CurrentLevel { get; }
    public ExpSystemData(float exp, int currentLevel)
    {
        this.Exp = exp;
        this.CurrentLevel = currentLevel;
    }
}
