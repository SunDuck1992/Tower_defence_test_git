using UnityEngine;

public class ArrayBulletShoot : Weapon
{
    private int _count;
    private int _spreadAngle = 5;

    protected override void CreateBullet(Bullet bullet)
    {
        _count++;

        if(_count > CountBullet)
        {
            _count = 0;
            WeaponPoint.localRotation = Quaternion.identity;
        }

        if(_count > 0)
        {
            WeaponPoint.localRotation = Quaternion.Euler(Vector3.up * (_count == 1 ? -_spreadAngle : _spreadAngle));
        }

            base.CreateBullet(bullet);
    }
}
