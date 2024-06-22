using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Health : MonoBehaviour
{

    public int health = 100; 
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetHealth((float)health / 100);
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

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
