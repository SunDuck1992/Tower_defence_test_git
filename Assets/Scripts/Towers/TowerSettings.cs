using System.Collections.Generic;
using UnityEngine;

namespace TowerSpace
{
    [CreateAssetMenu(fileName = nameof(TowerSettings), menuName = "GameData/" + nameof(TowerSettings))]
    public class TowerSettings : ScriptableObject
    {
        [SerializeField] private List<TowerData> _datas;

        public IReadOnlyList<TowerData> Datas => _datas;
    }
}