using UnityEngine;

public class Minion : MonoBehaviour
{
    public int maxHealth = 50;    // The maximum health of the minion
    private int currentHealth;     // The current health of the minion

    public float moveSpeed = 3f;   // Movement speed of the minion
    public float attackRange = 1.5f;  // Range at which the minion can attack the player
    public int damage = 10;         // Damage dealt by the minion
    public float attackCooldown = 1.5f; // Time between attacks
    private float lastAttackTime;

    private Transform player;      // Reference to the player's transform

    private void Start()
    {
        currentHealth = maxHealth;
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

    private void Update()
    {
        if (player == null)
            return;

        MoveTowardsPlayer();

        // If the minion is within attack range, attack the player
        if (Vector2.Distance(transform.position, player.position) <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move the minion towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        if (player != null)
        {
            player.GetComponent<Player>().ChangeHealth(-damage); // Damage the player
            Debug.Log("Minion attacked the player!");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Minion died!");
        Destroy(gameObject); // Destroy the minion game object
    }
}
