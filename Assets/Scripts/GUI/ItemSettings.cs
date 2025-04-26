using System;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class ItemSettings
    {
        [SerializeField] private int _cost;
        [SerializeField] private bool _isBuyed;

        public int Cost => _cost;
        public bool IsBuyed => _isBuyed;

        public void Buy()
        {
            _isBuyed = true;
        }
    }
}