using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] float horizontalAttackDistance = 1.2f;
    [SerializeField] float verticalAttackDistance = 1.5f;
    [SerializeField] public float knockbackForce = 20f;
    [SerializeField] public float stunTime = .5f;
    [SerializeField] public Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;


    private Vector2 facingDirection = new Vector2(-1, 0); // Default to left
    private float lastAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        // Trigger attack animation, sounds, etc.

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            enemy.GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, stunTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
    public void UpdateAttackPoint(Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            facingDirection = movementDirection.normalized;
            RepositionAttackPoint();
        }
    }

    private void RepositionAttackPoint()
    {
        Vector2 adjustedDistance = new Vector2(
            facingDirection.x * horizontalAttackDistance,
            facingDirection.y * verticalAttackDistance
        );

        attackPoint.localPosition = adjustedDistance;
    }
}
