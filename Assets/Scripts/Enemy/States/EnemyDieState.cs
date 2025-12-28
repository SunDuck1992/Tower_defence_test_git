using UnityEngine;
using StateSpace;

namespace EnemySpace
{
    public class EnemyDieState : BaseState<Enemy>
    {
        private float _timer = 1.5f;
        private int _typeDieCount = 2;
        private bool _isDied;

        public override void Enter()
        {
            _isDied = false;
            int typeDie = Random.Range(0, _typeDieCount);

            Owner.Agent.enabled = false;
            Owner.Animator.SetTrigger(AnimationConst.Die);
            Owner.Animator.SetInteger(AnimationConst.TypeDie, typeDie);
        }

        public override void Update()
        {
            if ((Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationConst.TypeDieSecond) ||
                 Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationConst.TypeDieFirst)) &&
                 !_isDied)
            {
                if (Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    _isDied = true;
                    Owner.CreateDeathParticle();
                }
            }

            if (_isDied)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    Owner.StateMachine.SwitchState<EnemyIdleState, Enemy>(Owner);
                }
            }
        }

        public override void Exit()
        {
            Owner.DiedCompleted.Invoke(Owner);
            Owner.SwitchFreezePartical(false);

            if (Owner.Target != null && Owner.TargetAttackPoint != null)
            {
                Owner.Target.AttackSector.freePoints.Push(Owner.TargetAttackPoint);
            }
        }
    }
}