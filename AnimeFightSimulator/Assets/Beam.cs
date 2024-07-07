using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Beam : MonoBehaviour
{
    public VisualEffect vfx;
    private bool active = false;
    public LayerMask beamMask;
    public int damageAmount = 10; // Damage dealt per particle collision
    private AttackAttributes _attackAttributes = new AttackAttributes();

    public float beamTime;
        // Update is called once per frame
    public void FireBeam()
    {
        vfx.gameObject.SetActive(true);
        vfx.Play();
        active = true;
        StartCoroutine(deactivateAfterTime());
    }


    private void Start()
    {
        _attackAttributes.damage = damageAmount;
        _attackAttributes.knockback = 100f;
    }

    private void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        RaycastHit hit;
        vfx.SetFloat("size",400);
                // Does the ray intersect any objects excluding the player layer
                if (Physics.SphereCast(transform.position, 0.5f,  transform.forward ,out hit, Mathf.Infinity, beamMask))
                {
                    
                    Damagable dm = hit.collider.GetComponentInParent<Damagable>();
                    if (dm != null) // Check if the object has the Enemy tag
                    {
                        _attackAttributes.direction = transform.forward;
                        dm.Damage(_attackAttributes);
                        vfx.SetFloat("size",hit.distance);
                    }
                }
    }

    IEnumerator deactivateAfterTime()
    {
        yield return new WaitForSeconds(beamTime);
        vfx.Stop();
        vfx.gameObject.SetActive(false);
        active = false;
    }
}
