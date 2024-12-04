using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UltimateAbility : MonoBehaviour
{
    [SerializeField] private int damageAmount = 100;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float suckInDistance = 15f;
    [SerializeField] private float damageDelay = 1f;
    [SerializeField] private float panSpawnDistance = 2f;
    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private float pullDuration = 1f;
    public bool canCastUlt;
    public GameObject panPrefab;

    private PlayerCombat playerCombat;
    private Player player;

    void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player.canCastUlt)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                CastUltimate();
                player.ChangeEnergy(-100);
            }
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
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (var enemy in allEnemies)
        {
            float distanceToPan = Vector3.Distance(enemy.transform.position, pan.transform.position);
            if (distanceToPan <= suckInDistance)
            {
                enemiesInRange.Add(enemy);
                DisableEnemyCollider(enemy);
                StartCoroutine(PullEnemyToPan(enemy, pan.transform));
                
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (pan != null) // Check if the pan is still valid before continuing
        {
            StartCoroutine(SlidePan(pan));
            yield return new WaitForSeconds(damageDelay);
            ApplyDamageToEnemies(enemiesInRange);
            Destroy(pan);
        }
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

    private IEnumerator PullEnemyToPan(GameObject enemy, Transform panTransform)
    {
        if (enemy == null || panTransform == null) yield break; // Check if enemy or pan is null

        float elapsedTime = 0f;

        while (elapsedTime < pullDuration)
        {
            if (enemy == null || panTransform == null) yield break; // Check if enemy or pan is null during the loop

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
}
