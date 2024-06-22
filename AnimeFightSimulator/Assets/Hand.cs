using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody)
        {
            other.rigidbody.AddExplosionForce(rb.velocity.magnitude*100f,other.contacts[0].point, 100f);
        }

        Damagable dam = other.gameObject.GetComponent<Damagable>();

        if (dam != null)
        {
           dam.Damage(  10*rb.velocity.magnitude); 
        }
    }
}
