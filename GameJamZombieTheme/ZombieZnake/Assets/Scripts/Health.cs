using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour , IDamagable
{
    //private
    public int healthpoints { get; private set; }
    public bool isAlive { get; private set; }

    private void Awake()
    {
        isAlive = true;
        healthpoints = 1;
    }

    public void ChangeHealth(int _value)
    {
        healthpoints += _value;

        if (healthpoints <= 0)
            Die();
    }

    public void Die()
    {
        isAlive = false;

        if (gameObject == PlayerController.instance.gameObject)
            UI_Manager.instance.ShowGameOver();

        StopAllCoroutines();
        Destroy(gameObject);
    }
}
