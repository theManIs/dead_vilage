using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "Resources/HeroAbilities/NewHeroAbility", menuName = "ScriptableObjects/HeroAbilityScriptable", order = 1)]
public class HeroAbilityScriptable : ScriptableObject
{
    public AbilityApplicationType OnSelf = AbilityApplicationType.OnSelf;
    public int StayingDuration = 1;
    public GameObject InstObject = null;
    public GameObject TemporaryEffect;
    [Header("Игровые сущности")]
    public int Cooldown;
    public int Damage;
    public int AreaRange;
    public int Healing;
    public int EnergyCost;

    public HeroAbilityScriptable()
    {
        TemporaryEffect = null;
    }
}
