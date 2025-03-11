using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowAction : MonoBehaviour
{
    public static ShowAction instance;

    [SerializeField] private List<ActionBase> _actionBases;
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textPlus;
    [SerializeField] private TextMeshProUGUI _textMinus;
    [SerializeField] private TextMeshProUGUI _textPrePlus;

    public void SetInfoAction(string name, int plus, int minus, int prePlus)
    {
        for (int i = 0; i < _actionBases.Count; i++)
        {
            if (_actionBases[i].name == name)
            {
                _actionBases[i].plus += plus;
                _actionBases[i].minus += minus;
                _actionBases[i].prePlus += prePlus;
            }
        }

        Show();
    }

    private void Show()
    {
        _textName.text = _actionBases[0].name;
        _textPlus.text = _actionBases[0].plus.ToString();
        _textMinus.text = _actionBases[0].minus.ToString();
        _textPrePlus.text = _actionBases[0].prePlus.ToString();
    }

    private void Awake()
    {
        instance = this;
    }
}

[Serializable]
public class ActionBase
{
    public string name;
    public int plus;
    public int minus;
    public int prePlus;
}

