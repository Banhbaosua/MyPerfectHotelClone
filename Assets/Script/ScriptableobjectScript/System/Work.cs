using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public abstract class Work : MonoBehaviour
{
    [SerializeField] WorkData data;
    [SerializeField] GameObject timerGO;
    [SerializeField] Image timerGOFill;
    [SerializeField] Collider workZone;
    CompositeDisposable disposables = new CompositeDisposable();
    private bool hasStarted;
    private bool isAvailable;
    private bool isDone;

    private float timer;

    public bool IsDone => isDone;
    public bool IsAvailable => isAvailable;

    public void Available(bool value)
    {
        isAvailable = value;
    }

    public virtual void WorkDone(bool value)
    {
        isDone = value;
    }

    protected virtual void Initiate()
    {
        TimerStreamInitiate();
    }

    void TimerStreamInitiate()
    {
        var workStreamAvailable = workZone.OnTriggerStayAsObservable().Where(_ => isAvailable);
        var workStreamNotAvailable = workZone.OnTriggerStayAsObservable().Where(_ => !isAvailable);
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
    protected virtual void Start()
    {
        Initiate();
    }

    private void OnDisable()
    {
        disposables?.Clear();
    }
}
