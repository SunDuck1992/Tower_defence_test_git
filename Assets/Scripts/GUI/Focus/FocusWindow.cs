using UnityEngine;
using YG;

public class FocusWindow : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;

    private void OnEnable()
    {
        YandexGame.onShowWindowGame += OnShowWindowGame;
        YandexGame.onHideWindowGame += OnHideWindowGame;
    }

    private void OnDisable()
    {
        YandexGame.onShowWindowGame -= OnShowWindowGame;
        YandexGame.onHideWindowGame -= OnHideWindowGame;
    }

    private void OnShowWindowGame()
    {
        _pauseManager.ResumeGame();
        AudioListener.volume = 1f;
    }

    private void OnHideWindowGame()
    {
        _pauseManager.PauseGame();
        AudioListener.volume = 0f;
    }
}
