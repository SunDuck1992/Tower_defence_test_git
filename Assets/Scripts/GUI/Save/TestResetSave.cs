using UnityEngine;
using YG;

public class TestResetSave : MonoBehaviour
{
    public void ResetSave()
    {
        YandexGame.ResetSaveProgress();
        YandexGame.SaveProgress();
    }
}
