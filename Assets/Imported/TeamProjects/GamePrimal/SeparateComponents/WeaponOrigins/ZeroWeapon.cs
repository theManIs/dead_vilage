using System;
using UnityEngine;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins
{
    class ZeroWeapon : WeaponOperatorAbstract
    {
        #region WeaponOperatorAbstract

        public override bool HasLastProjectile() => false;

        public override void SpawnProjectile(Transform projectile)
        {
            throw new NotImplementedException();
        }

        public override void ShootAnyProjectile(Transform enemy)
        {
            throw new NotImplementedException();
        }

        public override WeaponMathAbs GetWeaponMiscMath()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
