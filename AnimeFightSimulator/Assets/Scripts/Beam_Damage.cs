using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Beam_Damage : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt per particle collision
    private AttackAttributes _attackAttributes = new AttackAttributes();

    private void Start()
    {
        _attackAttributes.damage = damageAmount;
    }

    void OnParticleCollision(GameObject other)
    {
        Damagable dm = other.GetComponentInParent<Damagable>();
        if (dm != null) // Check if the object has the Enemy tag
        {
            dm.Damage(_attackAttributes);
        }
    }
}
