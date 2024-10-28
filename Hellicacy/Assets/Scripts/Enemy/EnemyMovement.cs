using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float attackRange = 1.2f;
    public LayerMask playerLayer;

    private EnemyState enemyState, newState;

    private Rigidbody2D rb;
    private Transform player;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;

        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if(enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();

            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                //attack
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

        if(Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            ChangeState(EnemyState.Attacking);
        }
        else if(Vector2.Distance(transform.position, player.position) > attackRange)
        {
           ChangeState(EnemyState.Chasing); 
        }    
        else
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
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
    Attacking,
    Knockback
}
