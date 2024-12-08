using System.Collections;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    public AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            StartCoroutine(BreakVase());
        }
    }

    private IEnumerator BreakVase()
    {
        audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}

