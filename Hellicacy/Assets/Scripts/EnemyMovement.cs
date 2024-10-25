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
                distance = Vector2.Distance(transform.position, player.transform.position);
                Vector2 direction = (player.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    IEnumerator PauseAndChaseRoutine()
{
    while (true)
    {
        while (enemyState == EnemyState.Knockback)
        {
            yield return null;
        }

        ChangeState(EnemyState.Idle);
        float pauseTime = Random.Range(minPauseDuration, maxPauseDuration);
        yield return new WaitForSeconds(pauseTime);

        while (enemyState == EnemyState.Knockback)
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
    if (newState == EnemyState.Knockback)
    {
        StopCoroutine(PauseAndChaseRoutine());
    }
    else if (enemyState == EnemyState.Knockback && newState != EnemyState.Knockback)
    {
        StartCoroutine(PauseAndChaseRoutine());
    }

    enemyState = newState;
}
}
public enum EnemyState
{
    Idle,
    Chasing,
    Knockback,
}
