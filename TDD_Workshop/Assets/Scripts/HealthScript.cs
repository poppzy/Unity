using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript
{
    private int health;
    private int maxHealth;

    public HealthScript(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public void AddHealth(int amount)
    {
        if (amount < 0)
        {
            amount *= -1;
        }

        health += amount;
    }

    public void RemoveHealth(int amount)
    {
        if (amount < 0)
        {
            amount *= -1;
        }

        health -= amount;
    }
}
