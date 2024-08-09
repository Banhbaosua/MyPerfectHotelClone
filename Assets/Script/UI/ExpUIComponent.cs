using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ExpUIComponent : MonoBehaviour
{
    [SerializeField] ExpSystem expSystem;
    [SerializeField] Slider slider;
    [SerializeField] Text text;
    CompositeDisposable disposables;
    private void Awake()
    {
        disposables = new CompositeDisposable();
        expSystem.OnExpReceive.Subscribe(UIUpdate).AddTo(disposables);
    }

    private void Start()
    {
        UIUpdate(expSystem.CurrentXp);
    }

    void UIUpdate(float exp)
    {
        slider.value = exp/expSystem.GetExpForLevel(expSystem.CurrentLevel + 1);
    }
    // Update is called once per frame
    private void OnDisable()
    {
        disposables?.Dispose();
    }
}
