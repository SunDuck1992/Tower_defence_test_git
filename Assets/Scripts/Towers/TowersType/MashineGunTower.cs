public class MashineGunTower : Tower
{
    public override void Enable()
    {
        StateMachine.SwitchState<IdleMashineGunTowerState, MashineGunTower>(this);
    }

    public override void Die()
    {
        StateMachine.SwitchState<DieMashineGunTowerState, MashineGunTower>(this);
    }
}
