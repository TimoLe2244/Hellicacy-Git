using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UltimateAbility : MonoBehaviour
{
    [SerializeField] public float energy;
    [SerializeField] public float maxEnergy;
    [SerializeField] public int damageAmount = 100;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float suckInDistance = 15f;
    [SerializeField] private float damageDelay = 1f;
    [SerializeField] private float panSpawnDistance = 2f;
    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private float pullDuration = 1f;
    [SerializeField] private float stunDuration = 1f;
    public GameObject panPrefab;

    private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>(); // Get the PlayerCombat component
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && energy >= maxEnergy)
        {
            CastUltimate();
        }
    }

    void CastUltimate()
    {
        Vector2 facingDirection = playerCombat.GetFacingDirection(); // Get the player's facing direction
        Vector3 panPosition = attackPoint.position + (Vector3)facingDirection * panSpawnDistance; // Spawn in facing direction
        GameObject pan = Instantiate(panPrefab, panPosition, Quaternion.identity);
        StartCoroutine(SuckEnemiesIntoPan(pan));
    }

    IEnumerator SuckEnemiesIntoPan(GameObject pan)
    {
    GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    List<GameObject> enemiesInRange = new List<GameObject>();

    foreach (var enemy in allEnemies)
    {
        float distanceToPan = Vector3.Distance(enemy.transform.position, pan.transform.position);
        if (distanceToPan <= suckInDistance)
        {
            enemiesInRange.Add(enemy);
            enemy.GetComponent<EnemyMovement>().Stun(stunDuration);
            
            // Disable enemy collider
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false; // Disable the collider
            }

            StartCoroutine(PullEnemyToPan(enemy, pan.transform));
        }
    }

    StartCoroutine(SlidePan(pan)); // Start sliding the pan immediately

    yield return new WaitForSeconds(damageDelay); // Delay before applying damage

    if (pan != null)
    {
        foreach (var enemy in enemiesInRange)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
            
            // Re-enable enemy collider after damage
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = true; // Re-enable the collider
            }
        }
    }

    Destroy(pan);
    energy = 0;
    }

    IEnumerator PullEnemyToPan(GameObject enemy, Transform panTransform)
    {
        float elapsedTime = 0f;

        while (elapsedTime < pullDuration && panTransform != null)
        {
            Vector3 directionToPan = (panTransform.position - enemy.transform.position).normalized;
            enemy.transform.position += directionToPan * pullStrength * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = panTransform.position;
    }

    IEnumerator SlidePan(GameObject pan)
    {
        Vector3 originalPosition = pan.transform.position;
        float slideDistance = 0.2f; // Increased distance to slide back and forth
        float slideDuration = 1f; // Increased duration of the sliding effect
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            float t = (elapsed / slideDuration);
            float offset = Mathf.Sin(t * Mathf.PI * 2) * slideDistance; // Smooth back and forth motion
            pan.transform.position = originalPosition + new Vector3(0, offset, 0); // Slide horizontally

            elapsed += Time.deltaTime;
            yield return null;
        }

        pan.transform.position = originalPosition; // Reset to original position
    }
}
