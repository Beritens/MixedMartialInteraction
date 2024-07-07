using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Damagable dm;
    private Rigidbody[] rbs;
    void Start()
    {
        dm = GetComponent<Damagable>();
        dm.OnDamaged += OnDamage;
        rbs = GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void OnDamage(object sender, AttackAttributes aa)
    {
        if (aa.explosionForce)
        {
            foreach (var rb in rbs)
            {
                rb.AddExplosionForce(aa.knockback,aa.origin,10, 0,ForceMode.Impulse);
            }
        }
        else
        {
            foreach (var rb in rbs)
            {
                rb.AddForce(aa.direction * aa.knockback, ForceMode.Impulse);
            }
        }
        
    }
}
