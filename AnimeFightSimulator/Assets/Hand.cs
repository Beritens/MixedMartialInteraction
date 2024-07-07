using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Rigidbody rb;
    public float force;
    private AttackAttributes _attackAttributes = new AttackAttributes();
    public bool left;
    public AttackTracker _attackTracker;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _attackAttributes.knockback = force;
    }


    private void OnCollisionEnter(Collision other)
    {
        // if (other.rigidbody)
        // {
        //     other.rigidbody.AddExplosionForce(force + rb.velocity.magnitude*force,other.contacts[0].point, 100f, 0, ForceMode.Impulse);
        // }

        Damagable dam = other.gameObject.GetComponentInParent<Damagable>();

        if (left? _attackTracker.getLeftAttacking() : _attackTracker.getRightAttacking() && dam != null)
        {
            _attackAttributes.damage = 10 * rb.velocity.magnitude;
            _attackAttributes.knockback = force * rb.velocity.magnitude;
            _attackAttributes.direction = rb.velocity.normalized;
           dam.Damage(  _attackAttributes); 
        }
    }
}
