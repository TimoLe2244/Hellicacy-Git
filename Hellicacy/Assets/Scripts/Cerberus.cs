using System.Collections;
using UnityEngine;

public class Cerberus : MonoBehaviour
{
    public int maxHealth = 1000;
    private int phaseThreshold1, phaseThreshold2;

    public GameObject[] minionPrefabs;
    public float minionSpawnDelay = 2f;
    public float spawnRadius = 5f;

    private MeleeEnemy meleeEnemyScript;
    private EnemyMovement enemyMovementScript;
    private EnemyKnockback enemyKnockbackScript;
    private Enemy enemyScript;

    public float speed = 2f;
    public float damage = 10f;

    private int minionCount = 0;
    private const int maxMinions = 5;

    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        phaseThreshold1 = maxHealth / 3;
        phaseThreshold2 = maxHealth / 3 * 2;

        meleeEnemyScript = GetComponent<MeleeEnemy>();
        enemyMovementScript = GetComponent<EnemyMovement>();
        enemyKnockbackScript = GetComponent<EnemyKnockback>();

        EnterPhase1();
    }

    void Update()
    {
        CheckHealthAndPhase();
    }

    private void CheckHealthAndPhase()
    {
        if (enemyScript.currentHealth <= phaseThreshold2)
        {
            if (meleeEnemyScript.enabled)
            {
                StopAllCoroutines();
            }
            EnterPhase2();
        }

        if (enemyScript.currentHealth <= phaseThreshold1)
        {
            if (meleeEnemyScript.enabled)
            {
                StopAllCoroutines();
            }
            EnterPhase3();
        }
    }

    private void EnterPhase1()
    {
        meleeEnemyScript.enabled = true;
        enemyMovementScript.enabled = true;
        enemyKnockbackScript.enabled = true;
        Debug.Log("Entering Phase 1: Normal Attacks");
    }

    private void EnterPhase2()
    {
        meleeEnemyScript.enabled = false;
        enemyMovementScript.enabled = true;
        enemyKnockbackScript.enabled = true;
        StartCoroutine(SpawnMinions());
        Debug.Log("Entering Phase 2: Minion Spawn");
    }

    private void EnterPhase3()
    {
        speed *= 1.5f;
        damage *= 2;
        Debug.Log("Entering Phase 3: Boss Does Double Damage, and Moves Faster");

        meleeEnemyScript.enabled = false;
        enemyMovementScript.enabled = true;
        enemyKnockbackScript.enabled = true;
    }

    IEnumerator SpawnMinions()
    {
        while (minionCount < maxMinions && enemyScript.currentHealth > phaseThreshold1)
        {
            GameObject randomMinionPrefab = minionPrefabs[Random.Range(0, minionPrefabs.Length)];

            Vector3 randomPosition = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;
            Instantiate(randomMinionPrefab, randomPosition, Quaternion.identity);

            minionCount++;
            yield return new WaitForSeconds(minionSpawnDelay);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        enemyScript.ChangeHealth(-damageAmount);
    }

    private void Die()
    {
        Debug.Log("Cerberus has been defeated!");
        Destroy(gameObject);
    }
}
