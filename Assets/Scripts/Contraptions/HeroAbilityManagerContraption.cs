using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Contraptions
{
    public class HeroAbilityManagerContraption
    {
        public HeroAbilityScriptable[] PassiveAbilities;
        public Transform SpawnPoint;
        public Dictionary<HeroAbilityScriptable, GameObject> SpawnedObjectsPassive = new Dictionary<HeroAbilityScriptable, GameObject>();
        public Dictionary<HeroAbilityScriptable, GameObject> SpawnedObjectsActive = new Dictionary<HeroAbilityScriptable, GameObject>();
        public Dictionary<KeyCode, HeroAbilityScriptable> ActiveAbilities = new Dictionary<KeyCode, HeroAbilityScriptable>();
        public HeroAbilityManagerContraption(HeroAbilityScriptable[] passiveAbilities, Transform spanPoint, HeroAbilityScriptable[] activeAbilities)
        {
            PassiveAbilities = passiveAbilities;
            SpawnPoint = spanPoint;

            if (activeAbilities.Length > 0)
            {
                ActiveAbilities.Add(KeyCode.Q, activeAbilities[0]);
            } 
            else if (activeAbilities.Length > 1)
            {
                ActiveAbilities.Add(KeyCode.W, activeAbilities[1]);
            } 
            else if (activeAbilities.Length > 2)
            {
                ActiveAbilities.Add(KeyCode.E, activeAbilities[2]);
            } 
            else if (activeAbilities.Length > 3)
            {
                ActiveAbilities.Add(KeyCode.R, activeAbilities[3]);
            }
        }

        private int _energyLevel = 0;

        public void RunPassive()
        {
            if (SpawnedObjectsPassive.Count > 0)
            {
                foreach (KeyValuePair<HeroAbilityScriptable, GameObject> spawnedObject in SpawnedObjectsPassive)
                {
                    Object.Destroy(spawnedObject.Value);
                    SpawnedObjectsPassive.Remove(spawnedObject.Key);
                }
            }

            if (PassiveAbilities.Length > 0)
            {
                foreach (HeroAbilityScriptable has in PassiveAbilities)
                {
                    if (has.InstObject && SpawnPoint)
                    {
                        SpawnedObjectsPassive[has] = Object.Instantiate(has.InstObject.gameObject, SpawnPoint);
                    }
                }
            }
        }

        public IEnumerator RunActive(KeyCode keyCodePressed)
        {
            if (ActiveAbilities.ContainsKey(keyCodePressed))
            {
                HeroAbilityScriptable has = ActiveAbilities[keyCodePressed];

                if (has && !SpawnedObjectsActive.ContainsKey(has) && has.StayingDuration > 0)
                {
                    if (_energyLevel - has.EnergyCost >= 0)
                    {
                        yield return has;

                        SpawnedObjectsActive[has] = Object.Instantiate(has.InstObject, SpawnPoint);

                        yield return new WaitForSeconds(has.StayingDuration);

                        Object.Destroy(SpawnedObjectsActive[has]);
                        SpawnedObjectsActive.Remove(has);
                    }
                }
            }


            yield return null;
        }

        public HeroAbilityManagerContraption SetEnergyValue(int energyCurrentLevel)
        {
            _energyLevel = energyCurrentLevel;

            return this;
        }
    }
}