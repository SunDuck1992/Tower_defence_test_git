using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackSector))]
public abstract class GameUnit : MonoBehaviour
{
    [SerializeField] protected float _maxHealth;

    private float _health;
    private bool _isDead;

    public UnityEvent<GameUnit> DiedComplete;
    public UnityEvent<GameUnit> DiedStart;
    public event Action OnDied;
    public event Action<bool> HealthChanged;

    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;
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
        if(_isDead) return;

        _health -= damage;
        HealthChanged?.Invoke(false);

        if (_health <= 0)
        {
            _isDead = true;
            OnDied?.Invoke();
            DiedStart.Invoke(this);
        }
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        _isDead = false;
        HealthChanged?.Invoke(true);
    }
}
