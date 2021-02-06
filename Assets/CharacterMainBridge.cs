using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMainBridge : MonoBehaviour
{
    public HealthKickerContraption HealthKickerContraption;
    public bool ClickOnEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        HealthKickerContraption = new HealthKickerContraption();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
