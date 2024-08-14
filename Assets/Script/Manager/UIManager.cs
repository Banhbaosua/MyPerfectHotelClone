using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour
{
    [SerializeField] CurrencySystem currencySystem;
    [SerializeField] Text cashText;

    private void Awake()
    {
        currencySystem.OnCashChange.Subscribe(x => UpdateCashUI(x));
    }

    void UpdateCashUI(float value)
    {
        cashText.text = value.ToString();
    }
}
