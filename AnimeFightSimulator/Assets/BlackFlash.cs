using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFlash : MonoBehaviour
{
    public bool left;

    public AttackTracker attackTracker;

    public ParticleSystem particles;

    public float flashRadius;

    public LayerMask enemies;
    // Start is called before the first frame update

    private AttackAttributes _attackAttributes = new AttackAttributes();
    public float damage;
    public float knockback;


    void Start()
    {
        _attackAttributes.knockback = knockback;
        _attackAttributes.damage = damage;
        _attackAttributes.explosionForce = true;
        if (left)
        {
            attackTracker.OnBlackFlashLeft += Attack;
        }
        else
        {
            
            attackTracker.OnBlackFlashRight += Attack;
        }
    }

    // Update is called once per frame
    void Attack(object sender, EventArgs args)
    {
        Debug.Log("black flash");
        particles.Play();
        Collider[] overlaps = Physics.OverlapSphere(transform.position, flashRadius, enemies);

        foreach (var other in overlaps)
        {
            Damagable dam = other.gameObject.GetComponentInParent<Damagable>();

            _attackAttributes.origin = transform.position;
            dam.Damage(  _attackAttributes); 
        }
    }
}
