using TowerSpace;

namespace StateSpace
{
    public class DieMashineGunTowerState : BaseState<MashineGunTower>
    {
        public override void Update()
        {
            Owner.StateMachine.SwitchState<IdleMashineGunTowerState, MashineGunTower>(Owner);
        }

        public override void Exit()
        {
            Owner.DiedCompleted.Invoke(Owner as Tower);
        }
    }
}