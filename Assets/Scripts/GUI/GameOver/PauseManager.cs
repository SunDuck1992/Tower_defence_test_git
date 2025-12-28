using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _pauseButtonWeapon;
        [SerializeField] private Button _pauseGamebookButton;
        [SerializeField] private Button _resumeLeaderBoardButton;
        [SerializeField] private Button _resumeAuthExitButton;
        [SerializeField] private Button _resumeAuthLeaderBoardButton;
        [SerializeField] private Button _resumeGameBookButton;
        [SerializeField] private Button _resumeShopCloseButton;
        [SerializeField] private Button _resumeWeapon1Button;
        [SerializeField] private Button _resumeWeapon2Button;

        private ButtonHandler _buttonHandler;

        [Inject]
        public void Construct(ButtonHandler buttonHandler)
        {
            _buttonHandler = buttonHandler;
        }

        private void OnEnable()
        {
            _buttonHandler.OnButtonClicked += PauseGame;
            _buttonHandler.OnButtonClicked += ResumeGame;
        }

        private void OnDisable()
        {
            _buttonHandler.OnButtonClicked -= PauseGame;
            _buttonHandler.OnButtonClicked -= ResumeGame;
        }

        private void Start()
        {
            ResumeGame();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void PauseGame(Button button)
        {
            if (_pauseButton || _pauseButtonWeapon || _pauseGamebookButton == button)
            {
                Time.timeScale = 0;
            }
        }

        public void ResumeGame()
        {
            if (_gameOverPanel.activeSelf)
            {
                return;
            }

            Time.timeScale = 1;
        }

        public void ResumeGame(Button button)
        {
            if (_resumeLeaderBoardButton || 
                _resumeAuthExitButton || 
                _resumeAuthLeaderBoardButton || 
                _resumeGameBookButton ||
                _resumeShopCloseButton ||
                _resumeWeapon1Button || 
                _resumeWeapon2Button == button)
            {
                if (_gameOverPanel.activeSelf)
                {
                    return;
                }

                Time.timeScale = 1;
            }
        }
    }
}