using TowerSpace;

namespace StateSpace
{
    public class DieRocketTowerState : BaseState<RocketTower>
    {
        public override void Update()
        {
            Owner.StateMachine.SwitchState<IdleRocketTowerState, RocketTower>(Owner);
        }

        public override void Exit()
        {
            Owner.DiedCompleted.Invoke(Owner);
        }
    }
}