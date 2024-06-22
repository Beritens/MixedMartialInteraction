using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    private Health _health;
    // Start is called before the first frame update
    void Start()
    {
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    public void Damage(float amount)
    {
        _health.ChangeHealth(-amount);
    }
}
