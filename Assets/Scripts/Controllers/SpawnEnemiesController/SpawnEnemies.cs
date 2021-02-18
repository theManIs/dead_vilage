using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemies : MonoBehaviour
{
    public CharacterMainBridge CharacterMainBridge;
    public CharacterMainBridge MainCharacterToPursue;
    public Transform[] SpawnPoints;
    public float EachSeconds = 20;
    public float CurrentTimer = 20;
    public List<CharacterMainBridge> EnemiesList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemiesList != null)
        {
            CurrentTimer += Time.deltaTime;

            if (CurrentTimer > EachSeconds)
            {
                CurrentTimer = 0;

                int intPoint = Convert.ToInt32(Random.value * (SpawnPoints.Length - 1));
                CharacterMainBridge spawnedZombie = Instantiate(CharacterMainBridge, SpawnPoints[intPoint]);
                spawnedZombie.HumanPlayer = MainCharacterToPursue;

//                Debug.Log(EnemiesList.Count);
                EnemiesList.Add(spawnedZombie);

            }
        }
    }
}
