using System;
using UnityEngine;
using UnityEngine.Events;
using Common;

namespace Unit
{
    [RequireComponent(typeof(AttackSector))]
    public abstract class GameUnit : MonoBehaviour
    {
        [SerializeField] protected float _maxHealth;

        private float _health;
        private bool _isDead;

        public UnityEvent<GameUnit> DiedCompleted;
        public UnityEvent<GameUnit> DiedStarted;
        public event Action Died;
        public event Action<bool> HealthChanged;

        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public AttackSector AttackSector { get; private set; }


        protected virtual void Awake()
        {
            AttackSector = GetComponent<AttackSector>();
        }

        private void OnEnable()
        {
            _health = _maxHealth;
            _isDead = false;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            _health -= damage;
            HealthChanged?.Invoke(false);

            if (_health <= 0)
            {
                _isDead = true;
                Died?.Invoke();
                DiedStarted.Invoke(this);
            }
        }

        public void ResetHealth()
        {
            _health = _maxHealth;
            _isDead = false;
            HealthChanged?.Invoke(true);
        }
    }
}