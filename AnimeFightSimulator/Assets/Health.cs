using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    private float health = 100;

    public event EventHandler<float> OnHealthChange; 


    private void Start()
    {
        health = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public float ChangeHealth(float amount)
    {
        health += amount;
        if (health <= 0)
        {
            Die();
        }
        OnHealthChange?.Invoke(this,health);
        return health;
    }

    public void Die()
    {
        //instead user observer pattern and handle death somewhere else
        Object.Destroy(gameObject);
    }
}
