using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    private float health = 100;

    public event EventHandler<float> OnHealthChange; 
    public event EventHandler onDeath;


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
        //don't damage the ghosts
        if (health < 0)
        {
            return health;
        }
        health += amount;
        if (health <= 0)
        {
            Die();
        }
        OnHealthChange?.Invoke(this,health);
        return health;
    }

    private void Die()
    {
        onDeath?.Invoke(this, EventArgs.Empty);

    }
    
    
}
