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

    private Player player;

    private Vector2 facingDirection = new Vector2(-1, 0); // Default facing direction for idle state
    private float lastAttackTime = 0f;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Update the attack point's direction based on the mouse position
        UpdateAttackPoint();

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

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().ChangeHealth(-attackDamage);
            player.GetComponent<Player>().ChangeEnergy(5);
            enemy.GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
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

    public void UpdateAttackPoint()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the z-position is set to 0 for 2D space

        // Calculate direction from player to mouse cursor
        facingDirection = (mousePosition - transform.position).normalized;

        // Update attack point position based on facing direction
        attackPoint.position = transform.position + new Vector3(facingDirection.x * horizontalAttackDistance, facingDirection.y * verticalAttackDistance, 0);
    }


    public Vector2 GetFacingDirection()
    {
        return facingDirection;
    }
}

