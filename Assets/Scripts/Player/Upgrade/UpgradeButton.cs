using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Upgrade _upgrade;
    [SerializeField] private int _maxLevelUpgrade;

    private int _countClicked;

    public event Action<Upgrade> Clicked;

    public int MaxLevelUpgrade => _maxLevelUpgrade;
    public int CountClicked => _countClicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(() =>
        {
            Clicked?.Invoke(_upgrade);
            _countClicked++;
        });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}
