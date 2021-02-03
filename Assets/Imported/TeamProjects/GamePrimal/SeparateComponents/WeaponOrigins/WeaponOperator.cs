using UnityEditor;
using UnityEngine;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins
{
    public class WeaponOperator : WeaponOperatorAbstract
    {
        #region Fields

        private Transform _lastProjectile; 

        #endregion


        #region WeaponOperatorAbstract

        public override bool HasLastProjectile() => _lastProjectile;

        public override void SpawnProjectile(Transform projectile)
        {
            if (_lastProjectile)
                Destroy(_lastProjectile.gameObject);

            GameObject newProjectile = Instantiate(projectile.gameObject);
            newProjectile.transform.position = SpawnPoint.position;
            newProjectile.transform.rotation = SpawnPoint.rotation;
            newProjectile.transform.parent = SpawnPoint.transform;
            _lastProjectile = newProjectile.transform;

            if (newProjectile.GetComponent<Rigidbody>())
                Destroy(newProjectile.GetComponent<Rigidbody>());
        }

        public override void ShootAnyProjectile(Transform enemy)
        {
            if (!_lastProjectile) return;

            Rigidbody rb = _lastProjectile.gameObject.AddComponent<Rigidbody>();
            rb.velocity = _lastProjectile.transform.forward * ShootPower;
            _lastProjectile = null;

            #if UNITY_EDITOR
            if (PauseAfterShoot)
                EditorApplication.isPaused = true; 
            #endif
        }

        public override WeaponMathAbs GetWeaponMiscMath() => new WeaponMiscMath().SetWRange(WeaponRange).SetWeapon(this);

        #endregion
    }
}
