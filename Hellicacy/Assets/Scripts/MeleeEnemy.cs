using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
  public int damage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public float attackWindUp = 0.5f;
    private float lastAttackTime;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(PerformMeleeAttack());
            lastAttackTime = Time.time;
        }
    }

    IEnumerator PerformMeleeAttack()
    {
        yield return new WaitForSeconds(attackWindUp);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            player.GetComponent<Player>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("Attack missed, player moved out of range.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
