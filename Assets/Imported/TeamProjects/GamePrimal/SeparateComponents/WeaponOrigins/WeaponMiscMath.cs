using System;
using UnityEditor;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.WeaponOrigins
{
    public class WeaponMiscMath : WeaponMathAbs
    {
        private Transform _enemyTransform;
        private Transform _allyTransform;
        private Vector3 _touchPoint;
        private WeaponOperatorAbstract _weaponOperator;
        private float _weaponRange;

        public override WeaponMiscMath SetEnemy(Transform enemy) => ReturnThis(() => _enemyTransform = enemy);

        public override WeaponMiscMath SetAlly(Transform ally) => ReturnThis(() => _allyTransform = ally);

        public override WeaponMiscMath SetTouchPoint(Vector3 touch) => ReturnThis(() => _touchPoint = touch);

        public override WeaponMiscMath SetWeapon(WeaponOperatorAbstract weapon) => ReturnThis(() => _weaponOperator = weapon);

        public override WeaponMiscMath SetWRange(float weaponRange) => ReturnThis(() => _weaponRange = weaponRange);

        private WeaponMiscMath ReturnThis<T>(Func<T> someFunc)
        {
            someFunc();

            return this;
        }

        public override bool InRange()
        {
//            if (_allyTransform && !0.0f.Equals(_weaponRange))
//                return Vector3.Distance(_enemyTransform.position, _allyTransform.position) < _weaponRange;

            if (_touchPoint != Vector3.zero && !0.0f.Equals(_weaponRange))
            {
                Vector3 direction = _touchPoint - _weaponOperator.transform.position;

                if (direction.sqrMagnitude > (_weaponRange * _weaponRange))
                    return false;

//                GameObject gm = new GameObject("Box for a distance checking");
                float enemyDist = Vector3.Distance(_weaponOperator.transform.position, _touchPoint);
//                gm.transform.position = _weaponOperator.transform.position;
//                gm.transform.localScale = new Vector3(1, 1, enemyDist);
//                Quaternion rotTarget = Quaternion.LookRotation(_touchPoint - _weaponOperator.transform.position);
////                gm.transform.LookAt(_enemyTransform.position, Vector3.up);
//                gm.transform.rotation = rotTarget;
//                gm.transform.localPosition = gm.transform.localPosition + gm.transform.forward * enemyDist / 2;
//                BoxCollider bc = gm.AddComponent<BoxCollider>();
//                Selection.activeGameObject = gm;
                
//                Ray ray = new Ray(_weaponOperator.transform.position, _touchPoint - _weaponOperator.transform.position);
                bool checkAddress = Physics.Raycast(_weaponOperator.transform.position, direction, enemyDist, 1 << 9);

//                if (checkAddress)
//                    Debug.Log($"checkAddress {checkAddress} boxExtents {new Vector3(1, 1, enemyDist) / 2} raycastHit {raycastHit.collider.gameObject.name}");
//                else
//                    Debug.Log($"checkAddress {checkAddress} boxExtents {new Vector3(1, 1, enemyDist) / 2} ");
//                EditorApplication.isPaused = true;
//                Debug.DrawRay(_weaponOperator.transform.position, _touchPoint - _weaponOperator.transform.position);
                return !checkAddress;
            }

            return false;
        }
    }
}