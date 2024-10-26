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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && energy >= maxEnergy) // Replace KeyCode.U with your preferred key
        {
            CastUltimate();
        }
    }

    void CastUltimate()
    {
        // Spawn the pan a few units away from the attack point
        Vector3 panPosition = attackPoint.position + attackPoint.up * panSpawnDistance; // Adjust height/direction as needed
        GameObject pan = Instantiate(panPrefab, panPosition, Quaternion.identity);
        StartCoroutine(SuckEnemiesIntoPan(pan));
    }

    IEnumerator SuckEnemiesIntoPan(GameObject pan)
    {
        // Find all enemies in the scene
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInRange = new List<GameObject>();

        // Move enemies toward the pan
        foreach (var enemy in allEnemies)
        {
            float distanceToPan = Vector3.Distance(enemy.transform.position, pan.transform.position);
            if (distanceToPan <= suckInDistance)
            {
                enemiesInRange.Add(enemy);
                enemy.GetComponent<EnemyMovement>().Stun(stunDuration);
                StartCoroutine(PullEnemyToPan(enemy, pan.transform)); // Use a new method for pulling
            }
        }

        // Wait for the specified pull duration before applying damage
        yield return new WaitForSeconds(damageDelay + pullDuration);

        // Check if the pan still exists before applying damage
        if (pan != null)
        {
            foreach (var enemy in enemiesInRange)
            {
                enemy.GetComponent<Enemy>().TakeDamage(damageAmount); // Adjust this to your enemy script
            }
        }

        // Destroy the pan after use
        Destroy(pan);
        energy = 0; // Reset energy after casting
    }

    IEnumerator PullEnemyToPan(GameObject enemy, Transform panTransform)
    {
        float elapsedTime = 0f;

        while (elapsedTime < pullDuration && panTransform != null)
        {
            // Calculate the pull direction and strength
            Vector3 directionToPan = (panTransform.position - enemy.transform.position).normalized;
            enemy.transform.position += directionToPan * pullStrength * Time.deltaTime; // Move enemy towards the pan

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the enemy is close enough to the pan at the end of pulling
        enemy.transform.position = panTransform.position;
    }
}



