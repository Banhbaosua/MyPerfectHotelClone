using System;
using UniRx.Triggers;
using UnityEngine;
using UniRx;

[Serializable]
public class Toilet : MonoBehaviour
{
    [SerializeField] Transform sitSpot;
    [SerializeField] int baseUseTimes;
    [SerializeField] Collider toiletDepositCol;
    [SerializeField] Transform paperIndicator;
    private int useTimes;
    private bool isAvailable;
    public bool IsAvailable => isAvailable && useTimes > 0;
    public Transform SitSpot => sitSpot;
    private void Awake()
    {
        Available();
        useTimes = baseUseTimes;

        toiletDepositCol.OnTriggerStayAsObservable().Subscribe(x =>
        {
            var player = x.GetComponent<CollectToiletPaper>();
            Debug.Log("deliver paper");
            ReplenishPaper();
            player.DepositPaper();
            toiletDepositCol.gameObject.SetActive(false);
            
        });
    }
    public void Available()
    {
        isAvailable = true;
    }

    public void Occupied()
    {
        isAvailable = false;
    }

    public void Used()
    {
        useTimes--;
        if (useTimes <= 0)
        {
            paperIndicator.gameObject.SetActive(true);
            toiletDepositCol.gameObject .SetActive(true);
        }
    }

    void ReplenishPaper()
    {
        useTimes = baseUseTimes;
        paperIndicator.gameObject.SetActive(false);
        toiletDepositCol.gameObject.SetActive(false);
    }
}
