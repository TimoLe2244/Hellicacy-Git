using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float attackRange = 1.2f;
    public LayerMask playerLayer;

    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Transform player;

    private bool canMove = false; // Track whether the enemy can move

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;

        // Start the coroutine to delay movement
        StartCoroutine(StartMovingAfterDelay(1f)); // 1 second delay
    }

    void Update()
    {
        if (enemyState != EnemyState.Knockback && canMove) // Only check for player if canMove is true
        {
            CheckForPlayer();

            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                // Attack
                rb.velocity = Vector2.zero;
            }
        }
    }

    void Chase()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void CheckForPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            ChangeState(EnemyState.Attacking);
        }
        else if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            ChangeState(EnemyState.Chasing);
        }
        else
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    private IEnumerator StartMovingAfterDelay(float delay)
    {
        // Set enemy state to Idle before starting the movement
        ChangeState(EnemyState.Idle);
        
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        
        // Allow movement after the delay
        canMove = true;
        
        // Change state to Chasing after the delay
        ChangeState(EnemyState.Chasing);
    }

    public void ChangeState(EnemyState newState)
    {
        enemyState = newState;
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback
}


