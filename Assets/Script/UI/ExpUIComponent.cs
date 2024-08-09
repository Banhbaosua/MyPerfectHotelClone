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
        expSystem.OnExpReceive.Subscribe(_=>UIUpdate()).AddTo(disposables);
    }

    private void Start()
    {
        UIUpdate();
    }

    void UIUpdate()
    {
        slider.value = expSystem.CurrentXp / expSystem.NextLevelExpRequired;
        text.text = expSystem.CurrentXp.ToString()+ "/" +expSystem.NextLevelExpRequired;
    }

    private void OnDisable()
    {
        disposables?.Dispose();
    }
}
