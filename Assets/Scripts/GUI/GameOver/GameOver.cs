using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();

        TryAgain();
    }

    public void TryAgain()
    {
        ResetTowerSave();
        SceneManager.LoadScene(0);
    }

    public void ShowRevardAd(int id)
    {
        YandexGame.RewVideoShow(id);
    }

    private void ResetTowerSave()
    {
        YandexGame.savesData.destroyedTowers.Clear();
        YandexGame.SaveProgress();
    }
}
