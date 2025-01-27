using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UltimateAbility : MonoBehaviour
{
    [Header("Ultimate Ability Settings")]
    [SerializeField] private int damageAmount = 100;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float suckInDistance = 15f;
    [SerializeField] private float damageDelay = 1f;
    [SerializeField] private float panSpawnDistance = 2f;
    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private float pullDuration = 1f;

    [Header("References")]
    public GameObject panPrefab;
    public bool canCastUlt;

    private PlayerCombat playerCombat;
    private Player player;

    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player.canCastUlt && Input.GetKeyDown(KeyCode.U))
        {
            CastUltimate();
            player.ChangeEnergy(-100);
        }
    }

    private void CastUltimate()
    {
        Vector2 facingDirection = playerCombat.GetFacingDirection();
        Vector3 panPosition = attackPoint.position + (Vector3)facingDirection * panSpawnDistance;
        GameObject pan = Instantiate(panPrefab, panPosition, Quaternion.identity);
        StartCoroutine(SuckEnemiesIntoPan(pan));
    }

    private IEnumerator SuckEnemiesIntoPan(GameObject pan)
    {
        List<GameObject> enemiesInRange = GetEnemiesInRange(pan);

        foreach (var enemy in enemiesInRange)
        {
            PullEnemyToPan(enemy, pan.transform);
        }

        yield return new WaitForSeconds(0.5f);

        if (pan != null)
        {
            StartCoroutine(SlidePan(pan));
            yield return new WaitForSeconds(damageDelay);
            ApplyDamageToEnemies(enemiesInRange);
            SendEnemiesFlying(enemiesInRange);
            Destroy(pan);
        }
    }

    private List<GameObject> GetEnemiesInRange(GameObject pan)
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (var enemy in allEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, pan.transform.position) <= suckInDistance)
            {
                enemiesInRange.Add(enemy);
                DisableEnemyCollider(enemy);
            }
        }

        return enemiesInRange;
    }

    private void DisableEnemyCollider(GameObject enemy)
    {
        if (enemy != null)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }
        }
    }

    private void PullEnemyToPan(GameObject enemy, Transform panTransform)
    {
        if (enemy == null || panTransform == null) return;

        StartCoroutine(PullEnemyCoroutine(enemy, panTransform));
    }

    private IEnumerator PullEnemyCoroutine(GameObject enemy, Transform panTransform)
    {
        float elapsedTime = 0f;

        while (elapsedTime < pullDuration)
        {
            if (enemy == null || panTransform == null) yield break;

            Vector3 directionToPan = (panTransform.position - enemy.transform.position).normalized;
            enemy.transform.position += directionToPan * pullStrength * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (enemy != null)
        {
            enemy.transform.position = panTransform.position;
        }
    }

    private IEnumerator SlidePan(GameObject pan)
    {
        if (pan == null) yield break;

        Vector3 originalPosition = pan.transform.position;
        float slideDistance = 0.5f;
        float slideDuration = 1f;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            if (pan == null) yield break;

            float t = elapsed / slideDuration;
            float offset = Mathf.Sin(t * Mathf.PI * 4) * slideDistance;
            pan.transform.position = originalPosition + new Vector3(0, offset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (pan != null)
        {
            pan.transform.position = originalPosition;
        }
    }

    private void ApplyDamageToEnemies(List<GameObject> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().ChangeHealth(-damageAmount);
                EnableEnemyCollider(enemy);
            }
        }
    }

    private void EnableEnemyCollider(GameObject enemy)
    {
        if (enemy != null)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = true;
            }
        }
    }

    private void SendEnemiesFlying(List<GameObject> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random direction
                float force = Random.Range(10f, 15f); // Random force for variety
                enemy.GetComponent<Rigidbody2D>().AddForce(randomDirection * force, ForceMode2D.Impulse);
            }
        }
    }
}

