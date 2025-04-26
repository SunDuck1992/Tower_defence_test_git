using System;
using UnityEngine;

namespace TowerSpace
{
    [Serializable]
    public struct TowerData
    {
        [SerializeField] private string _name;
        [SerializeField] private Tower _prefab;
        [SerializeField] private int _cost;
        [SerializeField] private Sprite _sprite;

        public Tower Prefab => _prefab;
        public int Cost => _cost;
        public Sprite Sprite => _sprite;
    }
}