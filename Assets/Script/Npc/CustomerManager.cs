using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [SerializeField] SpawnSystem spawnSystem;
    [SerializeField] RecipientController recipientController;
    [SerializeField] Transform startPos;
    CompositeDisposable disposables = new CompositeDisposable();
    Subject<Transform> onCustomerSpawnRequest;
    Subject<Transform> onCustomerDesSet;
    Subject<Unit> onCashSpawnRequest;
    public IObservable<Transform> OnCustomerSpawnResponse => onCustomerSpawnRequest;
    public IObservable<Transform> OnCustomerDesSet => onCustomerDesSet;
    public IObservable<Unit> OnCashSpawnRequest => onCashSpawnRequest;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
            DontDestroyOnLoad(this.gameObject);
        }

        onCustomerSpawnRequest = new Subject<Transform>();
        onCashSpawnRequest = new Subject<Unit>();
        onCustomerDesSet = new Subject<Transform>();
    }
    private void Start()
    {
        var checkAvailableStream = Observable.Interval(TimeSpan.FromSeconds(1f))
            .Select(_ => recipientController)
            .Where(x => x.WaitSlotAvailable != null)
            .Subscribe(x =>
            {
                onCustomerSpawnRequest.OnNext(x.WaitSlotAvailable.transform);
                x.WaitSlotAvailable.Occupied();
            }).AddTo(disposables);

        OnCustomerSpawnResponse.Subscribe(x =>
        {
            spawnSystem.Spawn<CustomerPool>(startPos);
            onCustomerDesSet.OnNext(x);
        }).AddTo(disposables);
    }

    private void OnDisable()
    {
        disposables?.Dispose();
    }
}
