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
    [SerializeField] LayerMask obstacleLayer; // Set the layer for walls/boundaries in the Inspector

    private bool canDash = true;
    private float lastDashTime;
    public float currentSpd;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
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

        if (movement != Vector2.zero)
        {
            playerCombat.UpdateAttackPoint(movement); // Update attack point direction
        }

        if (movement.x > 0)
        {
            sr.flipX = true;
        }
        else if (movement.x < 0)
        {
            sr.flipX = false;
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
            // Perform a raycast to check for obstacles in the dash direction
            RaycastHit2D hit = Physics2D.Raycast(rb.position, movement, dashAmount, obstacleLayer);

            // Calculate the dash distance based on any detected obstacle
            float dashDistance = hit.collider != null ? hit.distance : dashAmount;

            // Move the player only up to the dash distance, stopping at the wall if detected
            rb.MovePosition(rb.position + movement.normalized * dashDistance);
            
            isDash = false;
            lastDashTime = Time.time;
            canDash = false;

            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
