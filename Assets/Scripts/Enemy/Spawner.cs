using UnityEngine;
using YG;
using Zenject;
using Installers;

namespace EnemySpace
{
    public class Spawner : MonoBehaviour
    {
        private int _countEnemies;
        private int _enemyIncrement = 2;
        private int _firstGoldScaleLevel = 2;
        private int _secondGoldScaleLevel = 3;
        private int _startCountEnemies = 3;
        private int _firstLevelControlPoint = 5;
        private int _secondLevelControlPoint = 10;
        private EnemyManager _enemyManager;
        private SceneSettings _sceneSettings;

        public int CountEnemies => _countEnemies;
        public int WaveCount { get; private set; }
        public int MaxCountEnemies { get; private set; }

        [Inject]
        public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings)
        {
            _enemyManager = enemyManager;
            _sceneSettings = sceneSettings;
        }

        private void Start()
        {
            if (YandexGame.savesData.enemyCount == -1)
            {
                _countEnemies = _startCountEnemies;
            }
            else
            {
                _countEnemies = YandexGame.savesData.enemyCount;
            }

            if (YandexGame.savesData.waveCount == -1)
            {
                WaveCount = 0;
            }
            else
            {
                WaveCount = YandexGame.savesData.waveCount;
            }
        }

        public void SpawnOnClick()
        {
            for (int i = 0; i < _countEnemies; i++)
            {
                Transform point = _sceneSettings.Points[Random.Range(0, _sceneSettings.Points.Count)];
                _enemyManager.Create(point.position);
            }

            MaxCountEnemies = _countEnemies;
            WaveCount++;
            _countEnemies += _enemyIncrement;

            SetGoldScaleLevel();
        }

        private void SetGoldScaleLevel()
        {
            if (WaveCount >= _firstLevelControlPoint)
            {
                YandexGame.savesData.goldScaleLevel = _firstGoldScaleLevel;
            }

            if (WaveCount >= _secondLevelControlPoint)
            {
                YandexGame.savesData.goldScaleLevel = _secondGoldScaleLevel;
            }
        }
    }
}