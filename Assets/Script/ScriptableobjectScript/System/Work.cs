using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public abstract class Work : MonoBehaviour
{
    [SerializeField] WorkData data;
    [SerializeField] protected GameObject timerGO;
    [SerializeField] Image timerGOFill;
    [SerializeField] protected Collider workZone;
    protected CompositeDisposable disposables = new CompositeDisposable();
    private bool hasStarted;
    protected bool isAvailable;
    protected bool isDone;

    private float timer;

    Subject<Unit> onWorkDone;
    public IObservable<Unit> OnWorkDone => onWorkDone;
    protected IObservable<Collider> workStream;
    protected IObservable<Collider> workStreamAvailable;
    protected IObservable<Collider> workStreamNotAvailable;
    public bool IsDone => isDone;
    public bool IsAvailable => isAvailable;

    public void Available(bool value)
    {
        isAvailable = value;
    }

    public virtual void WorkDone(bool value)
    {
        isDone = value;
        onWorkDone.OnNext(Unit.Default);
    }

    protected virtual void Initiate()
    {
        onWorkDone = new Subject<Unit>();
        TimerStreamInitiate();
    }

    void TimerStreamInitiate()
    {
        workStream = workZone.OnTriggerStayAsObservable();
        workStreamAvailable = workStream.Where(_ => isAvailable);
        workStreamNotAvailable = workStream.Where(_ => !isAvailable);

        workStreamAvailable.Where(_ => !hasStarted)
            .Subscribe(_ =>
            {
                hasStarted = true;
                timer = 0;
                timerGO.SetActive(true);
            }).AddTo(disposables);

        workStreamAvailable.Where(_ => hasStarted)
            .Subscribe(_ =>
            {
                timerGO.SetActive(isAvailable);
                timer += Time.deltaTime;
                TimerUIUpdate(timer);
            }).AddTo(disposables);

        workStreamAvailable.Where(_ => timer >= data.WorkTime && !isDone)
            .Subscribe(_ =>
            {
                hasStarted = false;
                timerGO.SetActive(false);
                WorkDone(true);
            }).AddTo(disposables);

        workStreamNotAvailable.Where(_ => timerGO.activeSelf)
            .Subscribe(_ => timerGO.SetActive(false)).AddTo(disposables);

        workZone.OnTriggerExitAsObservable()
            .Subscribe(_=> timerGO.SetActive(false)).AddTo(disposables);
    }

    void TimerUIUpdate(float time)
    {
        timerGOFill.fillAmount = time/data.WorkTime;
    }
    protected virtual void Awake()
    {
        Initiate();
    }

    private void OnDisable()
    {
        disposables?.Clear();
    }
}
