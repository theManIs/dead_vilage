using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKickerContraption
{
    public int NormalDamage { get; } = 1;
    public int Health { get; private set; } = 2;

    private Action<int> WhenHit;

    public int hitMe(int amount)
    {
//        Debug.Log("hit me " + amount);
        Health = Health - amount;

        WhenHit(amount);

        return Health;
    }

    public void IAmHit(Action<int> toDoWhat)
    {
        WhenHit = toDoWhat;
    }

    public bool haveIKilled() => Health <= 0;

    public void SetHealth(int amount)
    {
        Health = amount;
    }
}
