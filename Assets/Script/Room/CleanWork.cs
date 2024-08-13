using UniRx;
using UniRx.Triggers;
using UnityEngine;
public class CleanWork : Work
{
    [SerializeField] Transform workIndicator;
    private void Start()
    {
        OnWorkDone.Subscribe(_ =>
        {
            isAvailable = false;
        });
    }

    private void FixedUpdate()
    {
        if(IsAvailable)
        {
            workIndicator.gameObject.SetActive(!timerGO.activeSelf);
        }
    }
}
