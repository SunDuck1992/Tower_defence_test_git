using UnityEngine;
using StateSpace;

namespace EnemySpace
{
    public class EnemyAttackState : BaseState<Enemy>
    {
        private int _typeAttackCount = 3;

        public override void Enter()
        {
            int typeAttack = Random.Range(0, _typeAttackCount);

            Owner.transform.forward = Owner.Target.transform.position - Owner.transform.position;
            Owner.Animator.SetTrigger("Attack");
            Owner.Animator.SetInteger("TypeAttack", typeAttack);

            Owner.Listener.Attack.AddListener(OnAttack);
        }

        public override void Exit()
        {
            Owner.Listener.Attack.RemoveAllListeners();
        }

        private void OnAttack()
        {
            Owner.CreateHitParticle();
            Owner.Target.TakeDamage(Owner.Damage);
            Owner.Target.AttackSector.freePoints.Push(Owner.TargetAttackPoint);
            Owner.TargetAttackPoint = null;
            Owner.StateMachine.SwitchState<EnemyIdleState, Enemy>(Owner);
        }
    }
}