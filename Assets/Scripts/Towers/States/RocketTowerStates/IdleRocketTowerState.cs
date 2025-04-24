using UnityEngine;
using TowerSpace;

namespace StateSpace
{
    public class IdleRocketTowerState : BaseState<RocketTower>
    {
        private float _fireAngleTrajectory = 5f;
        private int _defeatDistance = 35;
        private int _speedRotation = 10;

        public override void Update()
        {
            var target = Owner.TargetController.GetTarget(Owner, _defeatDistance, true);

            if (target != null)
            {
                Vector3 direction = target.transform.position - Owner.transform.position;
                Vector3 rotation = Quaternion.Lerp(Owner.TransformTower.rotation, Quaternion.LookRotation(direction), _speedRotation * Time.deltaTime).eulerAngles;
                rotation.x = 0;
                rotation.z = 0;

                Owner.TransformTower.eulerAngles = rotation;

                float angle = Vector3.Angle(Owner.TransformTower.forward, direction.normalized);

                if (angle <= _fireAngleTrajectory)
                {
                    Owner.StateMachine.SwitchState<ShootRocketTowerState, RocketTower>(Owner, state => state.target = target);
                }
            }
        }
    }
}