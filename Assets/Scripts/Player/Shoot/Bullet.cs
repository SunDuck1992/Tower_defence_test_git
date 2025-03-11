using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private ParticleSystem _hitBulletParticle;

    private Transform _targetPosition;
    private bool _hasHitTarget;
    private Coroutine _coroutine;

    public float Damage { get; set; }

    public event Action<Bullet> Died;
    public event Action<Enemy> Hit;
    public event Action<Enemy> HitTower;

    private void OnEnable()
    {
        _hasHitTarget = false;
        _coroutine = StartCoroutine(SelfDestructionCoroutine());
        //Invoke("SelfDestraction", 5f);
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
        //StopAllCoroutines();
    }

    private void Update()
    {
        //Vector3 direction = (_targetPosition.position - transform.position);

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        float targetY = _targetPosition.position.y;
        float smoothY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * _speed);
        Vector3 newPosition = transform.position;
        newPosition.y = smoothY; 
        transform.position = newPosition; 
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            _hasHitTarget = true;
            Instantiate(_hitBulletParticle, enemy.DeathParticlePoint.position, Quaternion.identity);
            //ShowAction.instance.SetInfoAction("Hit", 0, 0, 1);

            Hit?.Invoke(enemy);
            HitTower?.Invoke(enemy);
            Died?.Invoke(this);
        }
    }

    public void GetTargetPosition(GameUnit target)
    {
        var enemyTarget = target as Enemy;
        _targetPosition = enemyTarget.BulletTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerShooter.Radius);
    }

    //private void SelfDestraction()
    //{
    //    Died?.Invoke(this);
    //}

    private IEnumerator SelfDestructionCoroutine()
    {
        yield return new WaitForSeconds(5);

        if (!_hasHitTarget)
        {
            Died?.Invoke(this);
            //ShowAction.instance.SetInfoAction("Hit", 0, 1, 0);
        }
    }
}
