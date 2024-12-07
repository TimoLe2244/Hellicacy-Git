using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damagePerSecond = 5;
    private float damageTimer = 0f;
    private bool playerInLava = false;
    public AudioSource burningSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInLava = true;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    burningSound.Play();
                    player.GetComponent<Player>().ChangeHealth(-5);
                }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInLava = false;
            damageTimer = 0f;
        }
    }

    private void Update()
    {
        if (playerInLava)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= .5f)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    burningSound.Play();
                    player.GetComponent<Player>().ChangeHealth(-damagePerSecond);
                }
                damageTimer = 0f;
            }
        }
    }
}
