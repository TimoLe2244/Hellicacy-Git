using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Transform player;
    
    public float minPauseDuration = 1f; // minimum time to pause
    public float maxPauseDuration = 2.5f; // maximum time to pause
    public float minChaseDuration = 3f; // minimum time to chase
    public float maxChaseDuration = 7f;
    private bool isChasing = true;

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

        StartCoroutine(PauseAndChaseRoutine());
    }

    void Update()
    {
        if (isChasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop enemy when not chasing
        }
    }

    IEnumerator PauseAndChaseRoutine()
    {
        while (true)
        {
            // Pause the enemy for a random duration
            isChasing = false;
            float pauseTime = Random.Range(minPauseDuration, maxPauseDuration);
            yield return new WaitForSeconds(pauseTime);

            // Chase the player for a random duration
            isChasing = true;
            float chaseTime = Random.Range(minChaseDuration, maxChaseDuration);
            yield return new WaitForSeconds(chaseTime);
        }
    }
}
