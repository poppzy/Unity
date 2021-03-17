using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    int healthpoints { get; }
    bool isAlive { get; }

    public void ChangeHealth(int _value);
    public void Die();
}
