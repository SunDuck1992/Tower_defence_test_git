using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using UnityEngine.SceneManagement;

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
