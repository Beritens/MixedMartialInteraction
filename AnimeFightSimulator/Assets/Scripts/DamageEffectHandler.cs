using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectHandler : MonoBehaviour
{
    public ParticleSystem damageParticles;  // Assign this in the Inspector
    public ParticleSystem deathParticles;
    private void Start()
    {
        // Assuming Damagable is attached to the same GameObject
        Damagable damagable = GetComponent<Damagable>();
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