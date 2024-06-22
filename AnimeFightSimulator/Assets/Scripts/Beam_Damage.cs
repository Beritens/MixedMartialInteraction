using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Beam_Damage : MonoBehaviour
{
    public int damageAmount = 10; // Damage dealt per particle collision

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy")) // Check if the object has the Enemy tag
        {
            var enemyHealth = other.GetComponent<Character_Health>(); // Assuming there is an EnemyHealth script
            if (enemyHealth != null)
            {
                enemyHealth.reduceHealth(damageAmount); // Apply damage
            }
        }
    }
}
