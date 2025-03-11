using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ami.BroAudio;
using YG;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _countBullet;
    [SerializeField] private float _firerate;
    [SerializeField] private float _damage;
    [SerializeField] private Transform _weaponPoint;
    [SerializeField] private IKControl _IKControls;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private SoundID _soundID;
    [SerializeField] private float _changeFirerateValue;
    [SerializeField] private float _changeDamageValue;

    private int _maxIncreaseFirerateLevel = 5;
    private int _maxIncreaseDamageLevel = 10;

    public Transform WeaponPoint => _weaponPoint;
    public float FireRate => _firerate;
    public int CountBullet => _countBullet;

    public event Action MaxLevelIncreased;

    public void Shoot(Bullet bullet)
    {
        if (bullet == null)
        {
            return;
        }

        CreateBullet(bullet);
        Instantiate(_particleSystem, _weaponPoint.position, Quaternion.LookRotation(_weaponPoint.forward));
        BroAudio.Play(_soundID);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _IKControls.enabled = true;
    }

    public void DeActivate()
    {
        gameObject?.SetActive(false);
        _IKControls.enabled = false;
    }

    public float ChangeFirerate(float UpgradeLevelValue)
    {
        float firerate;

        if (_maxIncreaseFirerateLevel >= UpgradeLevelValue)
        {
            firerate = _firerate - (_changeFirerateValue * UpgradeLevelValue);
        }
        else
        {
            firerate = _firerate - (_maxIncreaseFirerateLevel * _changeFirerateValue);
            MaxLevelIncreased?.Invoke();
        }
        
        return firerate;
    }

    public float ChangeDamage(float UpgradeLevelValue)
    {
        float damage;

        if (_maxIncreaseDamageLevel >= UpgradeLevelValue)
        {
            damage = _damage + (_changeDamageValue * UpgradeLevelValue);
        }
        else
        {
            damage = _damage + (_maxIncreaseDamageLevel * _changeDamageValue);
            MaxLevelIncreased?.Invoke();
        }

        return damage;
    }

    protected virtual void CreateBullet(Bullet bullet)
    {
        bullet.transform.position = _weaponPoint.position;
        bullet.transform.forward = _weaponPoint.forward;
        bullet.Damage = _damage;
    }
}
