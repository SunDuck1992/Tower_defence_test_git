using StateSpace;

namespace EnemySpace
{
    public class EnemyIdleState : BaseState<Enemy>
    {
        private int _maxRadius = 1000;

        public override void Enter()
        {
            Owner.Target = Owner.TargetController.GetTarget(Owner, _maxRadius);
        }

        public override void Update()
        {
            if (Owner.Target != null)
            {
                Owner.Agent.enabled = true;

                Owner.StateMachine.SwitchState<EnemyMovementState, Enemy>(Owner);
            }
        }
    }
}