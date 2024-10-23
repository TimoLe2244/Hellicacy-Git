using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage!");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death behavior here
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject);
    }
}
