using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death behavior here
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }

    /*private IEnumerator FlashWhite()
    {
        Color originalColor = spriteRenderer.color; // Store the original color
        spriteRenderer.color = Color.white; // Change to white

        yield return new WaitForSeconds(0.1f); // Wait for a short duration

        spriteRenderer.color = originalColor; // Reset to original color
    }
    */
}
