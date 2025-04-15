using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;
using Zenject;

public class WaveScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countWavetext;
    [SerializeField] private TextMeshProUGUI _countEnemiesProgressText;
    [SerializeField] private Slider _progressWaveBar;
    [SerializeField] private GameObject _backGroundMusic;

    private EnemyManager _enemyManager;
    private Spawner _spawner;
    private SceneSettings _sceneSettings;

    public UnityEvent WaveComplete;
    public event Action OnEndBattle;
    public event Action OnStartBattle;

    public bool IsBattle { get; private set; }
    public int WaveCount { get; private set; }

    [Inject]
    public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings)
    {
        _enemyManager = enemyManager;
        _spawner = sceneSettings.Spawner;
        _sceneSettings = sceneSettings;
    }

    private void Start()
    {
        ShowBuildAreas();

        _enemyManager.EnemyDied += UpdateProgressBar;
        OnEndBattle += ShowBuildAreas;
        OnEndBattle += DisableMusic;
        OnEndBattle += SaveLeaderData;
        OnEndBattle += SaveWaweInfo;
        OnEndBattle += SaveEnemyLevelUpgrade;
    }

    private void OnDestroy()
    {
        _enemyManager.EnemyDied -= UpdateProgressBar;
        OnEndBattle -= ShowBuildAreas;
        OnEndBattle -= DisableMusic;
        OnEndBattle -= SaveLeaderData;
        OnEndBattle -= SaveWaweInfo;
        OnEndBattle -= SaveEnemyLevelUpgrade;
    }

    public void StartBattle()
    {
        _spawner.SpawnOnClick();
        _countEnemiesProgressText.text = $"{0} / {_spawner.MaxCountEnemies}";
        _countWavetext.text = _spawner.WaveCount.ToString();
        WaveCount = _spawner.WaveCount;
        _progressWaveBar.maxValue = _spawner.MaxCountEnemies;
        _progressWaveBar.value = 0;
        IsBattle = true;
        OnStartBattle?.Invoke();

        for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
        {
            _sceneSettings.BuildPoints[i].gameObject.SetActive(false);
        }
    }

    private void UpdateProgressBar()
    {
        _progressWaveBar.value++;
        _countEnemiesProgressText.text = $"{_progressWaveBar.value} / {_spawner.MaxCountEnemies}";

        if (_progressWaveBar.value >= _spawner.MaxCountEnemies)
        {
            WaveComplete.Invoke();
            OnEndBattle?.Invoke();
            IsBattle = false;
        }
    }

    private void ShowBuildAreas()
    {
        if (_progressWaveBar.value >= _spawner.MaxCountEnemies)
        {
            for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
            {
                if (_sceneSettings.BuildPoints[i].WaveLevel <= _spawner.WaveCount)
                {
                    _sceneSettings.BuildPoints[i].gameObject.SetActive(true);
                }
            }
        }
    }

    private void SaveLeaderData()
    {
        YandexGame.savesData.leaderScore += 1;
    }

    private void SaveWaweInfo()
    {
        YandexGame.savesData.waveCount = _spawner.WaveCount;
        YandexGame.savesData.enemyCount = _spawner.CountEnemies;
    }

    private void DisableMusic()
    {
        _backGroundMusic.SetActive(false);
    }

    private void SaveEnemyLevelUpgrade()
    {
        if(YandexGame.savesData.upgradeEnemyLevel == -1)
        {
            YandexGame.savesData.upgradeEnemyLevel = 1;
        }
        else
        {
            YandexGame.savesData.upgradeEnemyLevel++;
        }
    }
}
