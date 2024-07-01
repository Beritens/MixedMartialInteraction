using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Rigidbody rb;
    public float force;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody)
        {
            other.rigidbody.AddExplosionForce(force + rb.velocity.magnitude*force,other.contacts[0].point, 100f, 0, ForceMode.Impulse);
        }

        Damagable dam = other.gameObject.GetComponent<Damagable>();

        if (dam != null)
        {
           dam.Damage(  10 + 10*rb.velocity.magnitude); 
        }
    }
}
