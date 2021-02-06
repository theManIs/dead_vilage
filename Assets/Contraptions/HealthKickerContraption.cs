using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKickerContraption
{
    public int NormalDamage { get; } = 1;
    public int Health { get; private set; } = 1;

    public int hitMe(int amount)
    {
        Debug.Log("hit me " + amount);
        Health = Health - amount;

         return Health;
    }

    public bool haveIKilled() => Health <= 0;
}
