using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage!");

        if (health > 0)
        {
            StartCoroutine(FlashWhite()); // Start the flash coroutine
        }
        else
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

    private IEnumerator FlashWhite()
    {
        Color originalColor = spriteRenderer.color; // Store the original color
        spriteRenderer.color = Color.white; // Change to white

        yield return new WaitForSeconds(0.1f); // Wait for a short duration

        spriteRenderer.color = originalColor; // Reset to original color
    }
}
