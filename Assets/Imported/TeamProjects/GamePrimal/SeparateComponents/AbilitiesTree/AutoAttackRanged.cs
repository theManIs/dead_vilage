using System;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public class AutoAttackRanged : AbstractWeaponBasedRanged
    {
        public Transform ActualProjectile = Resources.Load<Transform>(ResourcesList.ArrowProjectile);
        public override bool HasProjectiles() => true;
        public override Transform GetProjectile() => ActualProjectile;
    }
}
