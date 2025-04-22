using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private WaveScreen _waveScreen;
    [SerializeField] private List<Button> _buttons;

    private void OnEnable()
    {
        _waveScreen.OnEndBattle += OnTurnOffTheButton;
    }

    private void OnDisable()
    {
        _waveScreen.OnEndBattle -= OnTurnOffTheButton;
    }

    public void TurnOnTheButton()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = true;
        }
    }

    private void OnTurnOffTheButton()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = false;
        }
    }
}
