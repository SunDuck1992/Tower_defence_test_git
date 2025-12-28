using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<ArrayKeeper> _buttonsWithIndex;

    public event Action<Button> OnButtonClicked;
    public event Action<Button, int> OnButtonClickedWithIndex;

    private void Start()
    {
        if(_buttons != null && _buttons.Count > 0)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                int index = i;
                _buttons[i].onClick.AddListener(() => HandleButtonClick(_buttons[index]));
            }

            for (int i = 0; i < _buttonsWithIndex.Count; i++)
            {
                int index = i;
                _buttonsWithIndex[i].WeaponButton.onClick.AddListener(() => HandleButtonClickWithIndex(_buttonsWithIndex[index].WeaponButton, _buttonsWithIndex[index].Index));
            }
        }
    }

    private void HandleButtonClick(Button button)
    {
        OnButtonClicked?.Invoke(button);
    }

    private void HandleButtonClickWithIndex(Button button, int index)
    {
        OnButtonClickedWithIndex?.Invoke(button, index);
    }
}
