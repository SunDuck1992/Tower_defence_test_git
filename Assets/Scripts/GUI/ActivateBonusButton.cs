using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class ActivateBonusButton : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private int _cost;

    private PlayerWallet _playerWallet;
    private WaveScreen _waveScreen;
    private Button _button;
    private bool _isButtonPressed = false;

    public UnityEvent<int> EnableBonus;
    public UnityEvent<int> DisableBonus;

    public bool IsActive { get; private set; }

    [Inject]
    public void Construct(PlayerWallet playerWallet, WaveScreen waveScreen)
    {
        _playerWallet = playerWallet;
        _waveScreen = waveScreen;
    }

    private void OnEnable()
    {
        _playerWallet.GemChanged += InteractButton;
        _waveScreen.OnStartBattle += CheckInteractButton;
    }

    private void OnDisable()
    {
        _playerWallet.GemChanged -= InteractButton;
        _waveScreen.OnStartBattle += CheckInteractButton;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void ActivateBonus()
    {
        if (!_isButtonPressed)
        {
            _isButtonPressed = true;

            StartCoroutine(StartTimer());

            EnableBonus.Invoke(_cost);
            IsActive = true;
        }
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_duration);

        _isButtonPressed = false;
        StopCoroutine(StartTimer());

        DisableBonus.Invoke(_cost);
        IsActive = false;
    }

    private void InteractButton(int countGem)
    {
        if (_waveScreen.IsBattle)
        {
            if (_cost > countGem)
            {
                _button.interactable = false;
            }
            else
            {
                _button.interactable = true;
            }
        }
        else
        {
            return;
        }
    }

    private void CheckInteractButton()
    {
        if (_waveScreen.IsBattle)
        {
            if (_cost > _playerWallet.Gem)
            {
                _button.interactable = false;
            }
            else
            {
                _button.interactable = true;
            }
        }
        else
        {
            return;
        }
    }
}
