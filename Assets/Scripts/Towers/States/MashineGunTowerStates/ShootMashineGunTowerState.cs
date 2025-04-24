using Unit;
using EnemySpace;
using PlayerSpace;
using TowerSpace;

namespace StateSpace
{
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

            _bullet.HitedTower += OnHit;
            _bullet.Died += OnBulletComplete;

            Owner.StateMachine.SwitchState<ReloadMashineGunTowerState, MashineGunTower>(Owner);
        }

        private void OnHit(Enemy enemy)
        {
            enemy.TakeDamage(_bullet.Damage);
        }

        private void OnBulletComplete(Bullet bullet)
        {
            bullet.HitedTower -= OnHit;
            bullet.Died -= OnBulletComplete;
        }
    }
}