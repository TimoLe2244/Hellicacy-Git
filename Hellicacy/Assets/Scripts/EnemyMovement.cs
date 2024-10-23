using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Transform player;
    public EnemyState enemyState;
    public float minPauseDuration = .2f; // minimum time to pause
    public float maxPauseDuration = 1f; // maximum time to pause
    public float minChaseDuration = 3f; // minimum time to chase
    public float maxChaseDuration = 7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if(playerObj != null)
        {
            player = playerObj.transform;
        }
        else{
            Debug.LogError("Player not found!");
        }
        ChangeState(EnemyState.Idle);
        StartCoroutine(PauseAndChaseRoutine());
    }

    void Update()
    {
        if(enemyState != EnemyState.Knockback)
        {
            if (enemyState == EnemyState.Chasing)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * speed;
            }
            else
            {
                rb.velocity = Vector2.zero; // Stop enemy when not chasing
            }
        }
    }

    IEnumerator PauseAndChaseRoutine()
    {
        while (true)
        {
            // Pause the enemy for a random duration
            ChangeState(EnemyState.Idle);
            float pauseTime = Random.Range(minPauseDuration, maxPauseDuration);
            yield return new WaitForSeconds(pauseTime);

            // Chase the player for a random duration
            ChangeState(EnemyState.Chasing);
            float chaseTime = Random.Range(minChaseDuration, maxChaseDuration);
            yield return new WaitForSeconds(chaseTime);
        }
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
    Knockback,
}
