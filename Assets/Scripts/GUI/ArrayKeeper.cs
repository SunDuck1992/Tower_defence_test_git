using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ArrayKeeper : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int _index;

    public int Index => _index;
    public Button WeaponButton => _button;
}
