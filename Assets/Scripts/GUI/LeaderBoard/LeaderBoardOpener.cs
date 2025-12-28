using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.LB;
using Zenject;

namespace UI
{
    public class LeaderBoardOpener : MonoBehaviour
    {
        [SerializeField] private GameObject _leaderBoard;
        [SerializeField] private GameObject _authWindow;
        [SerializeField] private LeaderboardYG _leaderboardYG;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private Button _authWindowButton;

        private ButtonHandler _buttonHandler;

        [Inject]
        public void Construct(ButtonHandler buttonHandler)
        {
            _buttonHandler = buttonHandler;
        }

        private void OnEnable()
        {
            _buttonHandler.OnButtonClicked += CheckAuthLeader;
            _buttonHandler.OnButtonClicked += Authorization;
        }

        private void OnDisable()
        {
            YandexGame.onGetLeaderboard -= OnCheckLeader;
            _buttonHandler.OnButtonClicked -= CheckAuthLeader;
            _buttonHandler.OnButtonClicked -= Authorization;
        }

        public void CheckAuthLeader(Button button)
        {
            if (_leaderBoardButton == button)
            {
                if (YandexGame.auth)
                {
                    _leaderBoard.SetActive(true);

                    YandexGame.onGetLeaderboard += OnCheckLeader;
                }
                else
                {
                    _authWindow.SetActive(true);
                }
            }
        }

        public void Authorization(Button button)
        {
            if(_authWindowButton == button)
            {
                YandexGame.AuthDialog();
            }           
        }

        private void OnCheckLeader(LBData lBData)
        {
            if (lBData.thisPlayer.score < YandexGame.savesData.leaderScore)
            {
                _leaderboardYG.NewScore(YandexGame.savesData.leaderScore);
                _leaderboardYG.UpdateLB();
            }
        }
    }
}