using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectHandler : MonoBehaviour
{
    public ParticleSystem damageParticles;  // Assign this in the Inspector
    public ParticleSystem deathParticles;
    public AudioSource audioSource;
    public AudioClip audioClip;
    private Health _health;
    private Knockback knockback;
    public Attractor attractor;
    private void Start()
    {
        // Assuming Damagable is attached to the same GameObject
        Damagable damagable = GetComponent<Damagable>();
        _health = GetComponent<Health>();
        _health.onDeath += HandleDeath;
        knockback = GetComponent<Knockback>();
        if (damagable != null)
        {
            damagable.OnDamaged += HandleDamage;
        }
    }

    private void HandleDamage(object sender, AttackAttributes aa)
    {
        if (damageParticles != null)
        {
            damageParticles.Play();
        }
    }

    private void HandleDeath(object sender, EventArgs args)
    {
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

        audioSource.PlayOneShot(audioClip);
        attractor.enabled = false;
        // Schedule explosion effect after delay
        Invoke(nameof(PlayExplosionEffect), 0.3f);
        Destroy(gameObject, 1);
    }

    private void OnDestroy()
    {
        // Clean up to avoid memory leaks
        Damagable damagable = GetComponent<Damagable>();
        if (damagable != null)
        {
            damagable.OnDamaged -= HandleDamage;
        }
    }
    
    public void PlayExplosionEffect()
    {
        if (deathParticles != null)
        {
            deathParticles.Play();
        }
    }

    
}