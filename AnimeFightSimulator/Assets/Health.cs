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
    public Knockback knockback;
    public DamageEffectHandler damageEffectHandler;
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

        // Create AttackAttributes for death scenario
        AttackAttributes aa = new AttackAttributes {
            damage = 0, // No damage necessary, just effects
            knockback = 50f, // Example value, adjust based on your game's physics
            origin = transform.position, // Centered on this GameObject
            explosionForce = true, // Use explosion force for a more dramatic effect
            direction = Vector3.up // Direct knockback upwards or adjust as needed
        };

        // Trigger knockback effect with attributes
        knockback?.OnDamage(this, aa);

        // Schedule explosion effect after delay
        Invoke("PlayExplosionEffect", 0.3f);

        // Delay the destruction to ensure effects are played
        UnityEngine.Object.Destroy(gameObject, 0.5f);
    }
    
    void PlayExplosionEffect()
    {
        damageEffectHandler?.PlayExplosionEffect();
    }
    
}
