using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    private Health _health;
    public float cooldown = 0.2f;
    private bool damagable = true;

    public event EventHandler<AttackAttributes> OnDamaged;
    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    public void Damage(AttackAttributes aa)
    {
        if (!damagable)
        {
            return;
        }
        _health.ChangeHealth(-aa.damage);
        OnDamaged?.Invoke(this, aa);
        StartCoroutine(WaitForCooldown());
    }
    IEnumerator WaitForCooldown()
    {
        damagable = false;
        yield return new WaitForSeconds(cooldown);
        damagable = true;
    }
}
