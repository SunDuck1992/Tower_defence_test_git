using System;
using UnityEngine;
using TMPro;

namespace UI
{
    [Serializable]
    public class LocalizationFont
    {
        [SerializeField] private string languageCode;
        [SerializeField] private TMP_FontAsset font;
    }
}