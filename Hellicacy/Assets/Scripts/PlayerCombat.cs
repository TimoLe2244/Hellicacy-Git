using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] float horizontalAttackDistance = 1.2f;
    [SerializeField] float verticalAttackDistance = 1.5f;
    [SerializeField] public float knockbackForce = 20f;
    [SerializeField] public float stunTime = .3f;
    [SerializeField] public float knockbackTime = .15f;

    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] public Transform attackPoint;
    public AudioSource audioSource;
    public AudioClip attackSound;


    private Vector2 facingDirection = new Vector2(-1, 0);
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
        audioSource.PlayOneShot(attackSound);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        if (hitEnemies.Length > 0)
        {
            hitEnemies[0].GetComponent<Enemy>().ChangeHealth(-attackDamage);
            hitEnemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
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

    public Vector2 GetFacingDirection()
{
    return facingDirection;
}
}
