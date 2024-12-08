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
    [SerializeField] private LayerMask vaseLayer;
    public AudioSource audioSource;
    public AudioClip attackSound;

    public float healingAmount;

    private Player player;
    private devil devilScript;

    private Vector2 facingDirection = new Vector2(-1, 0);
    private float lastAttackTime = 0f;

    void Start()
    {
        devilScript = FindObjectOfType<devil>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        UpdateAttackPoint();

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        if (attackPoint == null)
        {
            Debug.LogError("Attack point is not assigned.");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.ChangeHealth(-attackDamage);

                if (GameManager.Instance.playerChoice == 1 || GameManager.Instance.playerChoice == 3)
                {
                    healingAmount = attackDamage * 0.2f;
                    if (GameManager.Instance.IsBetterEffectActive())
                    {
                        healingAmount *= 1.5f;
                    }

                    player.ChangeHealth((int)healingAmount);
                    Debug.Log($"Player healed for {healingAmount} health!");
                }

                player.ChangeEnergy(2);
            }
            else
            {
                Debug.LogError("No Enemy component found on " + enemy.name);
            }
        }

        Collider2D[] hitVases = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, vaseLayer);
        foreach (Collider2D vase in hitVases)
        {
            Vase vaseScript = vase.GetComponent<Vase>();
            if (vaseScript != null)
            {
                vaseScript.ChangeHealth(-attackDamage);
            }
            else
            {
                Debug.LogError("No Vase component found on " + vase.name);
            }
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
        mousePosition.z = 0f;

        facingDirection = (mousePosition - transform.position).normalized;

        attackPoint.position = transform.position + new Vector3(facingDirection.x * horizontalAttackDistance, facingDirection.y * verticalAttackDistance, 0);
    }


    public Vector2 GetFacingDirection()
    {
        return facingDirection;
    }

}

