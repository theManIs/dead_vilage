using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins
{
    public abstract class WeaponMathAbs
    {


        #region Methods

        public abstract WeaponMiscMath SetEnemy(Transform enemy);

        public abstract WeaponMiscMath SetAlly(Transform ally);

        public abstract WeaponMiscMath SetTouchPoint(Vector3 touch);

        public abstract WeaponMiscMath SetWeapon(WeaponOperatorAbstract weapon);

        public abstract WeaponMiscMath SetWRange(float weaponRange);

        public abstract bool InRange(); 

        #endregion


    }
}