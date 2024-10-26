using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private float distance;
    private Transform player;
    public EnemyState enemyState;
    public float minPauseDuration = .2f;
    public float maxPauseDuration = .5f;
    public float minChaseDuration = 3f;
    public float maxChaseDuration = 7f;
    public float minimumDistance = .5f;

    public bool isStunned; // Track if the enemy is currently stunned
    public float stunDuration = 1f; // Duration for which the enemy is stunned

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        ChangeState(EnemyState.Idle);
        StartCoroutine(PauseAndChaseRoutine());
    }

    void Update()
    {
        if (isStunned)
        {
            rb.velocity = Vector2.zero; // Stop movement while stunned
            return; // Exit the update method
        }

        if (enemyState == EnemyState.Chasing)
        {
            distance = Vector2.Distance(transform.position, player.position);

            if (distance > minimumDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator PauseAndChaseRoutine()
    {
        while (true)
        {
            while (enemyState == EnemyState.Knockback || isStunned)
            {
                yield return null;
            }

            ChangeState(EnemyState.Idle);
            float pauseTime = Random.Range(minPauseDuration, maxPauseDuration);
            yield return new WaitForSeconds(pauseTime);

            while (enemyState == EnemyState.Knockback || isStunned)
            {
                yield return null;
            }

            ChangeState(EnemyState.Chasing);
            float chaseTime = Random.Range(minChaseDuration, maxChaseDuration);
            yield return new WaitForSeconds(chaseTime);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == EnemyState.Knockback || isStunned)
        {
            StopCoroutine(PauseAndChaseRoutine());
        }
        else if ((enemyState == EnemyState.Knockback || isStunned) && newState != EnemyState.Knockback && newState != EnemyState.Stunned)
        {
            StartCoroutine(PauseAndChaseRoutine());
        }

        enemyState = newState;
    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true; // Set the enemy to stunned
            ChangeState(EnemyState.Stunned); // Change the state to Stunned
            StartCoroutine(StunCoroutine(duration)); // Start the stun coroutine
        }
    }

    private IEnumerator StunCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration); // Wait for the stun duration
        isStunned = false; // Reset stunned state
        ChangeState(EnemyState.Idle); // Return to idle after stun
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Knockback,
    Stunned, // New state for stunned enemies
}

