using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RoomManager : MonoBehaviour, ILoadSavable
{
    [SerializeField] RoomsData roomsData;
    [SerializeField] List<SleepingRoom> upgradableRooms;
    [SerializeField] UpgradeBoard upgradeBoard;
    private int currentUpgradableIndex;
    CompositeDisposable disposables = new CompositeDisposable();
    CompositeDisposable boardDispoables = new CompositeDisposable();
    public void Load()
    {
        currentUpgradableIndex = SaveGame.Load("RoomManager", 0);
        upgradableRooms[currentUpgradableIndex].EnableUpdate();
    }

    public void Save()
    {
        SaveGame.Save("RoomManager", currentUpgradableIndex);
    }


    private void Awake()
    {
        InitiateRoomsData();
    }

    private void Start()
    {
        foreach (SleepingRoom room in upgradableRooms)
        {
            room.OnRoomUpgrade.Subscribe(_ =>
            {
                var room = upgradableRooms[currentUpgradableIndex];
                upgradeBoard.SetUpgradeData(room.CurrentRoomTierData);
                upgradableRooms[currentUpgradableIndex].DisableUpdate();
                if (currentUpgradableIndex < upgradableRooms.Count-1)
                    currentUpgradableIndex++;
                else
                    currentUpgradableIndex = 0;

                upgradableRooms[currentUpgradableIndex].EnableUpdate();

                upgradeBoard.gameObject.SetActive(true);
                Time.timeScale = 0;
                boardDispoables?.Clear();

                upgradeBoard.OnBtnRoom1Click
                .Subscribe(_ =>
                { 
                    room.RoomTypeByRoomTier[room.CurrentRoomTierData].room1.gameObject.SetActive(true);
                    Time.timeScale = 1;
                    upgradeBoard.gameObject.SetActive(false);
                })
                .AddTo(boardDispoables);

                upgradeBoard.OnBtnRoom2Click
                .Subscribe(_ =>
                {
                    room.RoomTypeByRoomTier[room.CurrentRoomTierData].room2.gameObject.SetActive(true);
                    Time.timeScale = 1; ;
                    upgradeBoard.gameObject.SetActive(false);
                })
                .AddTo(boardDispoables);

                Save();
            }).AddTo(disposables);
        }
    }
    private void OnDisable()
    {
        disposables?.Dispose();
    }
    // Update is called once per frame
    void InitiateRoomsData()
    {
        Load();
        roomsData.Init();
        foreach(Room room in this.transform.GetComponentsInChildren<Room>())
        {
            roomsData.AddRoom(room);
        }
    }
}
