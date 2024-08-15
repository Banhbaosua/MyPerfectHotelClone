using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UpgradeBoard : MonoBehaviour
{
    [SerializeField] Image img_Room1;
    [SerializeField] Image img_Room2;
    [SerializeField] Button btn_Room1;
    [SerializeField] Button btn_Room2;
    private RoomTierData tierData;
    public IObservable<Unit> OnBtnRoom1Click => btn_Room1.OnClickAsObservable();
    public IObservable<Unit> OnBtnRoom2Click => btn_Room2.OnClickAsObservable();
    public void SetUpgradeData(RoomTierData tierData)
    {
        img_Room1.sprite = tierData.Img_Room1;
        img_Room2.sprite = tierData.Img_Room2;
        this.tierData = tierData;
    }

    public void ChooseRoom()
    {

    }
}
