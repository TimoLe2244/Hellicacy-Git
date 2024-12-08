using System.Collections;
using UnityEngine;

public class TankEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Charging,
        KnockedBack
    }

    public EnemyState currentState = EnemyState.Idle;
    public int chargeDamage = 20;
    public float knockbackForce = 5f;
    private Rigidbody2D rb;

    [Header("Charging Parameters")]
    public float chargeTime = 2f;
    public float chargeCooldown = 3f;
    public float chargeSpeed = 10f;
    public float chargeColorTime = 1f;

    private Coroutine chargeCoroutine;
    private Transform playerTransform;
    private Vector2 chargeDirection;
    private SpriteRenderer spriteRenderer;
    private bool canStartCharging = false;
    private bool hasDamagedPlayer = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(WaitBeforeCharging());
    }

    void Update()
    {
        if (canStartCharging && currentState == EnemyState.Idle && chargeCoroutine == null)
        {
            chargeCoroutine = StartCoroutine(ChargeRoutine());
        }

        if (currentState == EnemyState.Charging)
        {
            ChargeMovement();
        }
    }

    private IEnumerator WaitBeforeCharging()
    {
        yield return new WaitForSeconds(2f);
        canStartCharging = true;
    }

    private IEnumerator ChargeRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(ChargePreparation());
            yield return new WaitForSeconds(chargeTime);
            StopCharging();
            yield return new WaitForSeconds(chargeCooldown);
        }
    }

    private IEnumerator ChargePreparation()
    {
        float elapsedTime = 0f;
        Color initialColor = spriteRenderer.color;
        Color targetColor = Color.red;

        while (elapsedTime < chargeColorTime)
        {
            spriteRenderer.color = Color.Lerp(initialColor, targetColor, elapsedTime / chargeColorTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
        StartCharging();
    }

    public void StartCharging()
    {
        currentState = EnemyState.Charging;
        chargeDirection = (playerTransform.position - transform.position).normalized;
        hasDamagedPlayer = false;
    }

    public void StopCharging()
    {
        currentState = EnemyState.Idle;
        rb.velocity = Vector2.zero;
        spriteRenderer.color = Color.white;
    }

    private void ChargeMovement()
    {
        rb.velocity = chargeDirection * chargeSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasDamagedPlayer)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.ChangeHealth(-chargeDamage);
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                player.TakeKnockback(knockbackDirection, knockbackForce);

                hasDamagedPlayer = true;
            }
        }
    }
}

