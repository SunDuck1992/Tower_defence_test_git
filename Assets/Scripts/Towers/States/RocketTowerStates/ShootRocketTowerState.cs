using System.Linq;
using UnityEngine;
using Unit;
using EnemySpace;
using TowerSpace;

namespace StateSpace
{
    public class ShootRocketTowerState : BaseState<RocketTower>
    {
        public GameUnit target;

        private Rocket _rocket;
        private float _multyplieMassiveDamage = 0.5f;
        private float _damageRadius = 3f;

        public override void Enter()
        {
            PushRocket();
        }

        private bool IsSpawnAreaClear()
        {
            Collider[] colliders = Physics.OverlapSphere(Owner.ShotPoint.position, 1f);
            return !colliders.Any(c => c.GetComponent<Rocket>() != null);
        }

        private void PushRocket()
        {
            if (IsSpawnAreaClear())
            {
                PrepareRocket();
            }

            Owner.StateMachine.SwitchState<ReloadRocketTowerState, RocketTower>(Owner);
        }

        private void PrepareRocket()
        {
            _rocket = Owner.RocketPool.Spawn();
            _rocket.Target = target;
            _rocket.Damage = Owner.Damage;
            _rocket.transform.position = Owner.ShotPoint.position;
            _rocket.transform.forward = Owner.ShotPoint.forward;

            _rocket.HitedTower += OnHit;
            _rocket.Died += OnRocketComplete;
        }

        private void OnHit(Enemy enemy)
        {
            enemy.TakeDamage(_rocket.Damage);

            var enemies = enemy.TargetController.GetAllTargets(enemy, _damageRadius, true);

            foreach (var unit in enemies)
            {
                if (unit != enemy)
                {
                    unit.TakeDamage(_rocket.Damage * _multyplieMassiveDamage);
                }
            }
        }

        private void OnRocketComplete(Rocket rocket)
        {
            _rocket.HitedTower -= OnHit;
            _rocket.Died -= OnRocketComplete;
        }
    }
}