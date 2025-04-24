using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "Config/" + nameof(PlayerConfig))]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _damage;

        public float Damage => _damage;
    }
}