using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMashineGunTowerState : BaseState<MashineGunTower>
{
    public GameUnit target;
    private Bullet _bullet;

    public override void Enter()
    {
        _bullet = Owner.BulletPool.Spawn();
        _bullet.GetTargetPosition(target);

        _bullet.transform.position = Owner.ShotPoint.position;
        _bullet.transform.forward = Owner.ShotPoint.forward;
        _bullet.Damage = Owner.Damage;

        Owner.CreateShootparticle();

        _bullet.HitTower += OnHit;
        _bullet.Died += BulletComplete;

        Owner.StateMachine.SwitchState<ReloadMashineGunTowerState, MashineGunTower>(Owner);
    }

    private void OnHit(Enemy enemy)
    {
        enemy.TakeDamage(_bullet.Damage);
    }

    private void BulletComplete(Bullet bullet)
    {
        bullet.HitTower -= OnHit;
        bullet.Died -= BulletComplete;
    }
}
