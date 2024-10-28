using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [SerializeField] int maxHealth = 100;
   [SerializeField] int currentHealth;
   private HealthBar healthBar;

   void Start()
   {
        currentHealth = maxHealth;
        healthBar = FindObjectOfType<HealthBar>();
   }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death
        Debug.Log("Player has died!");
    }
}
