using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
  public int damage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f; // Time between attacks
    public float attackWindUp = 0.5f; // Delay before attack lands
    private float lastAttackTime;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // Check if the player is within attack range and attack cooldown has passed
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(PerformMeleeAttack());
            lastAttackTime = Time.time;
        }
    }

    IEnumerator PerformMeleeAttack()
    {
        // Optional: Add an attack animation or sound cue here
        Debug.Log("Preparing melee attack...");

        // Wait for the wind-up time before executing the attack
        yield return new WaitForSeconds(attackWindUp);

        // Make sure the player is still in range after the wind-up (could have moved away)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Perform the actual attack
            Debug.Log("Melee attack hits!");
            player.GetComponent<Player>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("Attack missed, player moved out of range.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
