using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.SceneUtils;

public class TableOfEntities : MonoBehaviour
{
    public CharacterMainBridge MainPersonCharacter;
    public RPGCamera RpgCamera;
    public PlaceTargetWithMouse TargetAssistant;
    public SpawnEnemies SpawnEnemies;
    public List<CharacterMainBridge> EnemiesList;

    public void OnEnable()
    {
        RpgCamera.target = MainPersonCharacter.transform;
        TargetAssistant.setTargetOn = MainPersonCharacter.gameObject;

        if (MainPersonCharacter)
        {
            MainPersonCharacter.isHumanControl = true;
            MainPersonCharacter.GetComponent<NavMeshAgent>().speed = 1;
        }

        foreach (CharacterMainBridge characterMainBridge in EnemiesList)
        {
            characterMainBridge.HumanPlayer = MainPersonCharacter;
        }

        if (SpawnEnemies)
        {
            SpawnEnemies.EnemiesList = EnemiesList;
            SpawnEnemies.MainCharacterToPursue = MainPersonCharacter;
        }

    }
}
