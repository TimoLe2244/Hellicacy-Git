using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Range(1,25)]
    [SerializeField] private float speed = 10f;

    [Range(1,10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;
    public int damage = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate() {
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Player>().ChangeHealth(-damage);
                Debug.Log("Player took ranged damage");
            }

        Destroy(gameObject);
    }
}


