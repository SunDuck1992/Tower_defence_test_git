using UnityEngine;
using YG;
using Zenject;
using PlayerSpace;

namespace UI
{
    public class AdManager : MonoBehaviour
    {
        [SerializeField] private ShopScreen _shopScreen;
        [SerializeField] private GameOver _gameOver;

        private PlayerWallet _playerWallet;
        private int _gemCountRevard = 3;

        [Inject]
        public void Construct(PlayerWallet playerWallet)
        {
            _playerWallet = playerWallet;
        }

        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnRevarded;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnRevarded;
        }

        public void ShowFullScreenAd()
        {
            YandexGame.FullscreenShow();
        }

        public void CheckOrShowRevardAd(int id)
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