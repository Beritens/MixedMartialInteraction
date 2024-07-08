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
    public SoundHandler _soundHandler;

    public float beamTime;

    public bool IsActive()
    {
        return active;
    }
        // Update is called once per frame
    public void FireBeam()
    {
        vfx.gameObject.SetActive(true);
        vfx.Play();
        _soundHandler.PlayBeam();
        active = true;
        StartCoroutine(deactivateAfterTime());
    }


    private void Start()
    {
        _attackAttributes.damage = damageAmount;
        _attackAttributes.knockback = 100f;
        vfx.SetFloat("size",400);
    }

    private void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, Mathf.Infinity, beamMask);
        // Does the ray intersect any objects excluding the player layer
        foreach (var hit in hits)
        {
            
            Damagable dm = hit.collider.GetComponentInParent<Damagable>();
            if (dm != null) // Check if the object has the Enemy tag
            {
                _attackAttributes.direction = transform.forward;
                dm.Damage(_attackAttributes);
                //vfx.SetFloat("size",hit.distance);
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
