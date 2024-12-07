using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpd = 5f;
    [SerializeField] float sprintSpd = 8f;
    [SerializeField] bool canSprint = true;
    [SerializeField] bool isDash;
    [SerializeField] float dashAmount = 3f;
    [SerializeField] float dashCooldown = 2f;
    [SerializeField] LayerMask obstacleLayer;

    private bool canDash = true;
    private float lastDashTime;
    public float currentSpd;
    public Rigidbody2D rb;
    public SpriteRenderer[] characterSprites;
    Vector2 movement;

    private PlayerCombat playerCombat;

    void Awake()
    {
        playerCombat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            isDash = true;
        }

        if (movement.x > 0)
        {
            foreach (var sprite in characterSprites)
            {
                sprite.flipX = true;
            }
        }
        else if (movement.x < 0)
        {
            foreach (var sprite in characterSprites)
            {
                sprite.flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        currentSpd = TestIfSprinting() ? sprintSpd : walkSpd;
        rb.MovePosition(rb.position + movement.normalized * currentSpd * Time.fixedDeltaTime);

        if (isDash && canDash)
        {
            Dash();
        }
    }

    bool TestIfSprinting()
    {
        return canSprint && Input.GetKey(KeyCode.LeftShift);
    }

    void Dash()
    {
        if (canDash)
        {
            int originalLayer = gameObject.layer;

            gameObject.layer = LayerMask.NameToLayer("PlayerDash");

            RaycastHit2D hit = Physics2D.Raycast(rb.position, movement, dashAmount, obstacleLayer);
            float dashDistance = hit.collider != null ? hit.distance : dashAmount;

            rb.MovePosition(rb.position + movement.normalized * dashDistance);
            
            isDash = false;
            lastDashTime = Time.time;
            canDash = false;

            StartCoroutine(DashCooldown());
            StartCoroutine(RevertLayer(originalLayer));
        }
    }

    private IEnumerator RevertLayer(int originalLayer)
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.layer = originalLayer;
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
