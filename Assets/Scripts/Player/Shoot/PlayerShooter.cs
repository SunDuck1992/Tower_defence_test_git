using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using Zenject;
using Unit;
using Common;
using EnemySpace;
using Installers;
using Pool;
using UI;

namespace PlayerSpace
{
    public class PlayerShooter : MonoBehaviour
    {
        public const float Radius = 6f;
        public const float ShootDistance = 10f;

        [SerializeField] private Animator _weaponAnimator;
        [SerializeField] private List<Weapon> _weapons;
        [SerializeField] private Transform _view;
        [SerializeField] private Rotation _rotate;
        [SerializeField] private GameUnit _self;
        [SerializeField] private float _multyplieChangåCharacteristickValue;
        [SerializeField] private ParticleSystem _hitBulletParticle;
        [SerializeField] private ParticleSystem _massiveHitBulletParticle;

        private UISettings _uISettings;
        private WaveScreen _waveScreen;
        private float _damage;
        private BulletPool _bulletPool;
        private PlayerUpgradeSystem _playerUpgradeSystem;
        private bool _isMassiveDamage;
        private int _weaponIndex;
        private GameUnit _target;
        private TargetController _targetController;
        private PlayerWallet _playerWallet;
        private float _couldown;
        private float _currentDistance;
        private Coroutine _shootCourutine;

        public Weapon CurrentWeapon { get; private set; }
        public bool IsShooting { get; private set; }

        [Inject]
        public void Construct(BulletPool bulletPool, PlayerUpgradeSystem playerUpgradeSystem,
                              UISettings uISettings, TargetController targetController, PlayerWallet playerWallet, WaveScreen waveScreen)
        {
            _waveScreen = waveScreen;
            _targetController = targetController;
            _uISettings = uISettings;
            _bulletPool = bulletPool;
            _playerWallet = playerWallet;
            _playerUpgradeSystem = playerUpgradeSystem;

            YandexGame.GetDataEvent += OnSetCurrentWeapon;
            _waveScreen.BattlStarted += OnStartShooting;
            _waveScreen.BattleEnded += OnStopShooting;
            
            OnSetCurrentWeapon();

            _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged += OnUpdateDamage;
            _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.ValueChanged += OnUpdateShootSpeed;
            _uISettings.MassDamageButton.EnableBonus.AddListener(ActivateMassDamage);
            _uISettings.MassDamageButton.DisableBonus.AddListener(DeactivateMassDamage);

            ChangeWeapon(_weaponIndex);
            _couldown = CurrentWeapon.FireRate;
        }

        //~PlayerShooter()
        //{
        //    _uISettings.MassDamageButton.EnableBonus.RemoveListener(ActivateMassDamage);
        //    _uISettings.MassDamageButton.DisableBonus.RemoveListener(DeactivateMassDamage);
        //}

        private void Start()
        {
            OnUpdateShootSpeed();
            OnUpdateDamage();
            //StartCoroutine(Shoot());
        }

        private void Update()
        {
            if (_target != null)
            {
                Vector3 rotate = _target.transform.position - CurrentWeapon.WeaponPoint.transform.position;
                rotate.y = 0;
                _rotate.Direction = rotate;
                _rotate.IsShooting = true;
                _currentDistance = Vector3.Distance(_target.transform.position, gameObject.transform.position);
            }
            else
            {
                _currentDistance = 20f;
            }
        }

        private void OnDestroy()
        {
            _playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.ValueChanged -= OnUpdateDamage;
            _playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.ValueChanged -= OnUpdateShootSpeed;
            YandexGame.GetDataEvent -= OnSetCurrentWeapon;
            _waveScreen.BattlStarted -= OnStartShooting;
            _waveScreen.BattleEnded -= OnStopShooting;

            _uISettings.MassDamageButton.EnableBonus.RemoveListener(ActivateMassDamage);
            _uISettings.MassDamageButton.DisableBonus.RemoveListener(DeactivateMassDamage);
        }

        public void ChangeWeapon(int indexWeapon)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon?.DeActivate();
            }

            _weaponIndex = indexWeapon;
            CurrentWeapon = _weapons[_weaponIndex];
            CurrentWeapon.Activate();
            OnUpdateShootSpeed();
            OnUpdateDamage();
        }

        public void ActivateMassDamage(int cost)
        {
            if (_playerWallet.TrySpendGem(cost))
            {
                _isMassiveDamage = true;
            }
        }

        public void DeactivateMassDamage(int cost)
        {
            _isMassiveDamage = false;
        }

        public void OnUpdateShootSpeed()
        {
            _couldown = CurrentWeapon.ChangeFirerate(_playerUpgradeSystem.UpgradeData.UpgradeShootSpeedLevel.Value);
        }

        private void OnUpdateDamage()
        {
            _damage = CurrentWeapon.ChangeDamage(_playerUpgradeSystem.UpgradeData.UpgradeDamageLevel.Value);
        }

        private void OnHit(Enemy enemy)
        {
            if (_isMassiveDamage)
            {
                var enemies = _targetController.GetAllTargets(enemy, Radius, true);

                foreach (var e in enemies)
                {
                    if (e != enemy)
                    {
                        Instantiate(_massiveHitBulletParticle, enemy.DeathParticlePoint.position, Quaternion.identity);
                        e.TakeDamage(_damage * 0.7f);
                    }
                }
            }

            enemy.TakeDamage(_damage);
        }

        private void OnStartShooting()
        {
            _shootCourutine = StartCoroutine(Shoot());
        }

        private void OnStopShooting()
        {
            if(_shootCourutine != null)
            {
                StopCoroutine(_shootCourutine);
            }
        }

        private IEnumerator Shoot()
        {
            while (_waveScreen.IsBattle)
            {
                _target = _targetController.GetTarget(_self, 16, true);

                if (_target != null)
                {
                    if (_currentDistance < ShootDistance)
                    {
                        IsShooting = true;

                        for (int i = 0; i < CurrentWeapon.CountBullet; i++)
                        {
                            Bullet bullet = _bulletPool.Spawn();
                            bullet.GetTargetPosition(_target);
                            bullet.Hited += OnHit;
                            bullet.Died += OnBulletComplete;

                            CurrentWeapon.Shoot(bullet);
                        }

                        yield return new WaitForSeconds(_couldown);
                    }
                }
                else
                {
                    IsShooting = false;
                }

                yield return null;
            }
        }

        private void OnBulletComplete(Bullet bullet)
        {
            bullet.Hited -= OnHit;
            bullet.Died -= OnBulletComplete;
        }

        private void OnSetCurrentWeapon()
        {
            if (YandexGame.savesData.weaponIndex != -1)
            {
                _weaponIndex = YandexGame.savesData.weaponIndex;
            }
            else
            {
                _weaponIndex = 0;
            }

            ChangeWeapon(_weaponIndex);
        }
    }
}