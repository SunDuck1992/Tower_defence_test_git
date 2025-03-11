using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DieMashineGunTowerState : BaseState<MashineGunTower>
{
    public override void Update()
    {
        Owner.StateMachine.SwitchState<IdleMashineGunTowerState, MashineGunTower>(Owner);
    }

    public override void Exit()
    {
        Debug.LogWarning("Я умер башня");
        Owner.DiedComplete.Invoke(Owner as Tower);
    }
}
