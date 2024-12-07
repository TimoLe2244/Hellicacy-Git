using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damagePerSecond = 5; // The damage dealt per second
    private float damageTimer = 0f; // Timer to track the damage interval
    private bool playerInLava = false; // To check if the player is currently inside the lava

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the lava trigger, start applying damage
        if (other.CompareTag("Player"))
        {
            playerInLava = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Stop applying damage when the player exits the lava area
        if (other.CompareTag("Player"))
        {
            playerInLava = false;
            damageTimer = 0f; // Reset timer when the player exits the lava
        }
    }

    private void Update()
    {
        // Only apply damage if the player is in the lava
        if (playerInLava)
        {
            damageTimer += Time.deltaTime; // Increment the timer by the time passed since the last frame

            // Apply damage every second
            if (damageTimer >= .5f)
            {
                // Apply damage to the player and reset the timer
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.GetComponent<Player>().ChangeHealth(-damagePerSecond); // Deal damage to the player
                }
                damageTimer = 0f; // Reset the timer after applying damage
            }
        }
    }
}
