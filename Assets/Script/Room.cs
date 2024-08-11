using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] Collider doorColInside;
    [SerializeField] Collider doorColOutside;
    [SerializeField] float rotateDegree;

    private bool isPlayerInside = false;
    private bool isNPCInside;
    CompositeDisposable disposables = new CompositeDisposable();
    private void Awake()
    {
        Initiate();
        if(cameraController == null)
            cameraController = Camera.main.GetComponentInParent<CameraController>();
    }

    void Initiate()
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
