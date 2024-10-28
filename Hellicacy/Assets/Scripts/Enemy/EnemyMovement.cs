using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float attackRange = 1.2f;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private int facingDirection = -1;
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

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);
        

        if(hits.Length > 0)
        {
            player = hits[0].transform;

            if(Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                ChangeState(EnemyState.Attacking);
            }
            else if(Vector2.Distance(transform.position, player.position) > attackRange)
            {
               ChangeState(EnemyState.Chasing); 
            }    
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
