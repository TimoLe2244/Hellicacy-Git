using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [SerializeField] int maxHealth = 100;
   [SerializeField] int currentHealth;
   [SerializeField] int maxEnergy= 100;
   [SerializeField] int currentEnergy;
   private HealthBar healthBar;
   private EnergyBar energyBar;
   public bool canCastUlt;

   void Start()
   {
        currentHealth = maxHealth;
        healthBar = FindObjectOfType<HealthBar>();
        energyBar = FindObjectOfType<EnergyBar>();
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

    public void ChangeEnergy(int amount)
    {
        currentEnergy += amount;

        if (energyBar != null)
        {
            energyBar.UpdateEnergyBar(currentEnergy, maxEnergy);
        }

        if (currentEnergy >= 100)
        {
            currentEnergy = maxEnergy;
            canCastUlt = true;
        }
        else
        {
            canCastUlt = false;
        }
    }

    void Die()
    {
        // Handle player death
        Debug.Log("Player has died!");
    }
}
