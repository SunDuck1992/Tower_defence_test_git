using System;
using System.Collections;
using UnityEngine;
using Unit;
using EnemySpace;

namespace PlayerSpace
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private ParticleSystem _hitBulletParticle;

        private Transform _targetPosition;
        private bool _hasHitTarget;
        private Coroutine _coroutine;
        private int _flyTimeToDestruction = 5;

        public event Action<Bullet> Died;
        public event Action<Enemy> Hited;
        public event Action<Enemy> HitedTower;

        public float Damage { get; set; }

        private void OnEnable()
        {
            _hasHitTarget = false;
            _coroutine = StartCoroutine(SelfDestructionCoroutine());
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);
        }

        private void Update()
        {
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

                Hited?.Invoke(enemy);
                HitedTower?.Invoke(enemy);
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

        private IEnumerator SelfDestructionCoroutine()
        {
            yield return new WaitForSeconds(_flyTimeToDestruction);

            if (!_hasHitTarget)
            {
                Died?.Invoke(this);
            }
        }
    }
}