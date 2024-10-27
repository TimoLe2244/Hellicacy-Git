using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyMovement enemyMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void Knockback(Transform playerTransform, float knockbackForce, float stunTime)
    {
        // Only apply knockback if not currently stunned
        if (!enemyMovement.isStunned)
        {
            Vector2 direction = (transform.position - playerTransform.position).normalized;
            StartCoroutine(ApplyKnockback(direction, knockbackForce, stunTime));
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction, float knockbackForce, float stunTime)
    {
        // Apply knockback force
        rb.velocity = direction * knockbackForce;
        Debug.Log("Knockback applied");

        // Wait for a brief moment to allow the knockback to take effect
        yield return new WaitForSeconds(0.1f); // Adjust this duration as necessary

        // Then stun the enemy
        enemyMovement.ChangeState(EnemyState.Stunned);
        yield return new WaitForSeconds(stunTime);

        rb.velocity = Vector2.zero; // Stop movement after stun duration
        enemyMovement.ChangeState(EnemyState.Idle); // Return to idle state
    }
}
