using UnityEngine;
using UnityEngine.AI;
using StateSpace;

namespace EnemySpace
{
    public class EnemyMovementState : BaseState<Enemy>
    {
        private float _radius = 6f;
        private float _minDistance = 1.5f;
        private float _randomPointRadius = 2.5f;
        private float _actionDelay = 0.3f;
        private Vector3 _randomPoint;
        private float _timer;

        public override void Update()
        {
            if (Owner.Target.AttackSector.freePoints.Count == 0)
            {
                float distanceToTarget = Vector3.Distance(Owner.transform.position, Owner.Target.transform.position);

                if (distanceToTarget <= _radius)
                {
                    if (_timer <= Time.time)
                    {
                        _randomPoint = Owner.transform.position + Random.insideUnitSphere * _randomPointRadius;
                        _randomPoint.y = Owner.transform.position.y;
                        Owner.Agent.SetDestination(_randomPoint);
                        _timer = Time.time + _actionDelay;
                    }
                }

                return;
            }

            Owner.TargetAttackPoint = Owner.Target.AttackSector.freePoints.Peek();
            NavMesh.SamplePosition(Owner.TargetAttackPoint.position, out var hit, 1, NavMesh.AllAreas);
            Owner.Agent.SetDestination(hit.position);

            float distance = Vector3.Distance(Owner.transform.position, Owner.TargetAttackPoint.position);

            if (Owner.Agent.path.corners.Length > 1 & distance <= _minDistance)
            {
                Owner.TargetAttackPoint = Owner.Target.AttackSector.freePoints.Pop();
                Owner.StateMachine.SwitchState<EnemyAttackState, Enemy>(Owner);
            }
            else
            {
                Owner.Animator.SetBool(AnimationConst.IsGo, true);
            }
        }

        public override void Exit()
        {
            Owner.Animator.SetBool(AnimationConst.IsGo, false);

            Owner.Agent.SetDestination(Owner.transform.position);
        }
    }
}