using System.Collections;
using System.Collections.Generic;
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
        enemyMovement.ChangeState(EnemyState.Knockback);
        StartCoroutine(StunTimer(stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * knockbackForce;
        Debug.Log("knockback applied");
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        enemyMovement.ChangeState(EnemyState.Idle);
    }
}
