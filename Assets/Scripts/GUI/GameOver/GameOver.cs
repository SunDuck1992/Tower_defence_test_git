using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using Zenject;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private Button _endButton;
        [SerializeField] private Button _restartAdButton;

        private ButtonHandler _buttonHandler;

        [Inject]
        public void Construct(ButtonHandler buttonHandler)
        {
            _buttonHandler = buttonHandler;
        }

        private void OnEnable()
        {
            _buttonHandler.OnButtonClicked += RestartGame;
            _buttonHandler.OnButtonClicked += ShowRevardAd;
        }

        private void OnDisable()
        {
            _buttonHandler.OnButtonClicked -= RestartGame;
            _buttonHandler.OnButtonClicked -= ShowRevardAd;
        }

        public void RestartGame(Button button)
        {
            if (_endButton.name == button.name)
            {
                YandexGame.ResetSaveProgress();
                YandexGame.SaveProgress();

                TryAgain();
            }
        }

        public void TryAgain()
        {
            ResetTowerSave();
            SceneManager.LoadScene(0);
        }

        public void ShowRevardAd(Button button)
        {
            if (_restartAdButton.name == button.name)
            {
                YandexGame.RewVideoShow((int)RevardId.Revard3);
            }
        }

        private void ResetTowerSave()
        {
            YandexGame.savesData.destroyedTowers.Clear();
            YandexGame.SaveProgress();
        }
    }
}