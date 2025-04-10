using System.Collections;
using UnityEngine;

public abstract class Tower : GameUnit, IStateMachineOwner
{
    [SerializeField] private Transform _transformTower;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _shootDistance;
    [SerializeField] private ParticleSystem _healParticle;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _healParticalPoint;

    private float _improveDamage = 0.3f;
    private float _improveHealth = 2f;
    private float _currentDamage;
    private ParticleSystem _particle;
    private Coroutine _coroutine;
  
    public float ShootDistance => _shootDistance;
    public Transform TransformTower => _transformTower;
    public Transform ShotPoint => _shotPoint;
    public BuildArea BuildArea { get; set; }
    public float Damage => _currentDamage;
    public float FireRate => _fireRate;
    public IStateMachine StateMachine { get; set; }
    public TargetController TargetController { get; set; }
    public BulletPool BulletPool { get; set; }
    public RocketPool RocketPool { get; set; }

    protected override void Awake()
    {
        StateMachine = new StateMachine();
        base.Awake();
        _currentDamage = _damage;
    }

    private void Update()
    {
       StateMachine.UpdateState();
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        if(_particle != null)
        {
            Destroy(_particle.gameObject);
        }        
    }

    public virtual void Enable()
    {
    }

    public void Disable()
    {
    }

    public virtual void Die()
    {
    }

    public void ImproveTower(int level)
    {
        _currentDamage = _damage + (level * _improveDamage);
        _maxHealth = _maxHealth + (level * _improveHealth);

    }

    public void EnableHealParticle()
    {
         _particle = Instantiate(_healParticle, _healParticalPoint.position, Quaternion.identity);
        _particle.transform.localScale = _healParticalPoint.localScale;
        _particle.transform.rotation = _healParticalPoint.rotation;
        _particle.transform.SetParent(transform);

        _coroutine = StartCoroutine(DestroyParticleAfterDelay(_particle, 1.5f));
    }

    //public void DisableHealParticle()
    //{
    //    Destroy(_particle);
    //    _particle.gameObject.SetActive(false); 
    //}

    public void CreateShootparticle()
    {
        Instantiate(_shootParticle, ShotPoint.position, Quaternion.LookRotation(ShotPoint.forward));
    }

    public IEnumerator DestroyParticleAfterDelay(ParticleSystem particle, float delay)
    {       
        yield return new WaitForSeconds(delay);

        Destroy(particle.gameObject);
    }
}
