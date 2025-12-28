using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;
using Zenject;
using PlayerSpace;
using UnityEngine.UI;

namespace UI
{
    public class StoreResources : MonoBehaviour
    {
        private const string EnglishCode = "en";
        private const string RussianCode = "ru";
        private const string TurkishCode = "tr";

        [SerializeField] private int _costGem;
        [SerializeField] private TextMeshProUGUI _needMoreText;
        [SerializeField] private List<LocalizationFont> _localizationFonts;
        [SerializeField] private Button _gemForGoldButton;
        [SerializeField] private Button _gemForRewardButton;

        private PlayerWallet _playerWallet;
        private ButtonHandler _buttonHandler;
        private int _gemCount = 1;
        private float _duration = 2f;
        private Coroutine _coroutine;

        [Inject]
        public void Construct(PlayerWallet playerWallet, ButtonHandler buttonHandler)
        {
            _playerWallet = playerWallet;
            _buttonHandler = buttonHandler;
        }

        private void OnEnable()
        {
            _buttonHandler.OnButtonClicked += OnClickButtonSellGem;
            _buttonHandler.OnButtonClicked += OnClickButtonRewardGem;
        }

        private void OnDisable()
        {
            _buttonHandler.OnButtonClicked -= OnClickButtonSellGem;
            _buttonHandler.OnButtonClicked -= OnClickButtonRewardGem;
        }

        public void OnClickButtonSellGem(Button button)
        {
            if (_gemForGoldButton.name == button.name)
            {
                if (_playerWallet.TrySpendGold(_costGem))
                {
                    _playerWallet.AddGem(_gemCount);
                }
                else
                {
                    _coroutine = StartCoroutine(ChangeText());
                }
            }

        }

        public void OnClickButtonRewardGem(Button button)
        {
            if (_gemForRewardButton.name == button.name)
            {
                YandexGame.RewVideoShow((int)RevardId.Revard1);
            }
           
        }

        private IEnumerator ChangeText()
        {
            var nextTexts = _needMoreText;

            string text = nextTexts.text;
            TMP_FontAsset asset = nextTexts.font;

#if !UNITY_EDITOR
        string languageCode = YandexGame.lang;

        foreach(LocalizationFont localizationFont in _localizationFonts)
        {
            if(localizationFont.languageCode == languageCode)
            {
                nextTexts.font = localizationFont.font;
            }       
        }

        switch (languageCode)
        {
            case EnglishCode:
                nextTexts.text = "Need more gold";
                break;

            case RussianCode:
                nextTexts.text = "Нужно больше золота";
                break;

            case TurkishCode:
                nextTexts.text = "daha fazla altın lazım";
                break;

            default:
                nextTexts.text = "Need more gold";
                break;
        }
#endif

            nextTexts.color = Color.red;

            yield return new WaitForSeconds(0.5f);

            for (float t = _duration; t >= 0; t -= Time.deltaTime)
            {
                Color color = nextTexts.color;
                color.a = t;
                nextTexts.color = color;

                yield return null;
            }

            nextTexts.text = text;
            nextTexts.color = Color.white;
            nextTexts.font = asset;

            StopCoroutine(_coroutine);
        }
    }
}