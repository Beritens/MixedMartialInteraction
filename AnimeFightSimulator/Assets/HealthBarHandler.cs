using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    private Health _health;
    public HealthBar healthBar;
    private void Start()
    {
        _health = GetComponent<Health>();
        _health.OnHealthChange += OnHealthChange;
    }

    private void OnHealthChange(object sender, float health)
    {
        
        
        healthBar.SetHealth((float)health / _health.maxHealth);
    }
}
