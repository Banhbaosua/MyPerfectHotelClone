using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Room : MonoBehaviour, ILoadSavable
{
    [SerializeField] protected CurrencySystem currencySystem;
    [SerializeField] CameraController cameraController;
    [SerializeField] Collider doorColInside;
    [SerializeField] Collider doorColOutside;
    [SerializeField] float rotateDegree;
    private const int CASHPERDEPOSIT = 50;
    
    protected int currentDeposit;
    protected virtual int CashRequired { get; }
    
    private bool isPlayerInside = false;
    protected bool isAvailable = true;
    protected CompositeDisposable disposables = new CompositeDisposable();

    public virtual bool IsAvailable => isAvailable;
    protected virtual void Awake()
    {
        Initiate();
        if(cameraController == null)
            cameraController = Camera.main.GetComponentInParent<CameraController>();
    }

    protected virtual void Initiate()
    {
        var playerRoomEnterStream = doorColInside.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.CompareTag("Player") && !isPlayerInside)
            .Subscribe(_ =>
            {
                    cameraController.RotateCamera(new Vector3
                    (
                        cameraController.transform.rotation.eulerAngles.x,
                        rotateDegree,
                        cameraController.transform.rotation.eulerAngles.z)
                    );
                    isPlayerInside = true;
            }).AddTo(disposables);

        var playerRoomOutStream = doorColOutside.OnTriggerExitAsObservable()
            .Where(x => x.gameObject.CompareTag("Player") && isPlayerInside)
            .Subscribe(_ =>
            {
                    cameraController.ResetCamera();
                    isPlayerInside = false;
            }).AddTo(disposables);
    }
    
    public void Occupied()
    {
        isAvailable = false;
    }

    public void Available()
    {
        isAvailable = true;
    }

    public IEnumerator DepositeCash()
    {
        float baseSecond = 1;
        while (currentDeposit < CashRequired && currencySystem.Cash > 0)
        {
            int withdrawCash;
            if (CashRequired - CASHPERDEPOSIT < currentDeposit)
            {
                withdrawCash = currencySystem.RequestCash(CashRequired - currentDeposit);
            }
            else
            {
                withdrawCash = currencySystem.RequestCash(CASHPERDEPOSIT);
            }
            currentDeposit += withdrawCash;

            Save();
            yield return new WaitForSeconds(baseSecond);
            baseSecond /= 2;
        }
    }

    public virtual void Load()
    {
        
    }

    public virtual void Save()
    {
       
    }
}
