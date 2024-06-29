using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Health : MonoBehaviour
{
    public GameObject player; //For calculating Knockback direction
    public int health = 100;
    public HealthBar healthBar;
    
    private PlayerStats playerStats; // Reference to the PlayerStats script

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetHealth((float)health / 100);
        playerStats = player.GetComponent<PlayerStats>(); // Initialize the playerStats reference
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Check if Space key is pressed
        {
            reduceHealth(10);  // Reduce health by 10
        }
    }

    public void reduceHealth(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;

        healthBar.SetHealth((float)health / 100);

        if (health > 0 && gameObject.tag == "Enemy") //add knockback if object is tagged with enemy
        {
            ApplyKnockback(gameObject);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void ApplyKnockback(GameObject character)
    {
        Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
        knockbackDirection.y += 10; //add a little upward knockback
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * playerStats.knockbackForce, ForceMode.Impulse); // Use knockbackForce from PlayerStats
        }
    }
}