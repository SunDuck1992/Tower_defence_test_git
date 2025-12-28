using UnityEngine;
using YG;
using Zenject;
using PlayerSpace;
using UnityEngine.UI;

namespace UI
{
    public class AdManager : MonoBehaviour
    {
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private GameOver _gameOver;
        [SerializeField] private Button _weapon3BuyForADButton;

        private PlayerWallet _playerWallet;
        private ButtonHandler _buttonHandler;
        private int _gemCountRevard = 3;

        [Inject]
        public void Construct(PlayerWallet playerWallet, ButtonHandler buttonHandler)
        {
            _playerWallet = playerWallet;
            _buttonHandler = buttonHandler; 
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnRevarded;
            _buttonHandler.OnButtonClickedWithIndex += CheckOrShowRevardAd;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnRevarded;
            _buttonHandler.OnButtonClickedWithIndex -= CheckOrShowRevardAd;
        }

        public void CheckOrShowRevardAd(Button button, int id)
        {
            if(_weapon3BuyForADButton == button)
            {
                if (_shopScreen.CheckPurchaseItem(id))
                {
                    _shopScreen.ChangeWeaponButtonClick(id);
                    Time.timeScale = 1;
                }
                else
                {
                    YandexGame.RewVideoShow(id);
                }
            }           
        }

        private void OnRevarded(int id)
        {
            if (id == 1)
            {
                _playerWallet.AddGem(_gemCountRevard);
            }
            if (id == 2)
            {
                _shopScreen.ChangeWeaponButtonClick(id);
                _shopScreen.DisableADImage();
                YandexGame.savesData.miniGunIsBuyed = true;
            }
            if (id == 3)
            {
                _gameOver.TryAgain();
            }
        }
    }
}