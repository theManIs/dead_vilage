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
    public CharacterMainBridge[] EnemiesList;

    public void OnEnable()
    {
        RpgCamera.target = MainPersonCharacter.transform;
        TargetAssistant.setTargetOn = MainPersonCharacter.gameObject;

        MainPersonCharacter.isHumanControl = true;
        MainPersonCharacter.GetComponent<NavMeshAgent>().speed = 1;

        foreach (CharacterMainBridge characterMainBridge in EnemiesList)
        {
            characterMainBridge.HumanPlayer = MainPersonCharacter;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
