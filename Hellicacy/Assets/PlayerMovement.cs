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

    [SerializeField] Transform attackPoint; // Point where attacks are performed

    // Distances for the attack point on horizontal and vertical directions
    [SerializeField] float horizontalAttackDistance = 1.2f; 
    [SerializeField] float verticalAttackDistance = 1.5f;

    private bool canDash = true;
    private float lastDashTime; 
    public float currentSpd;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    Vector2 movement;

    // Store facing direction (initially set to left-facing)
    Vector2 facingDirection = new Vector2(-1, 0);

    void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Trigger dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            isDash = true;
        }

        // If there's movement, update the facing direction
        if (movement != Vector2.zero)
        {
            UpdateFacingDirection();
        }

        if (movement.x > 0)
        {
            sr.flipX = true;
        }
        else if (movement.x < 0)
        {
            sr.flipX=false;
        }
    }

    private void FixedUpdate()
    {
        // Update speed based on sprinting
        currentSpd = TestIfSprinting() ? sprintSpd : walkSpd;

        // Move the player
        rb.MovePosition(rb.position + movement.normalized * currentSpd * Time.fixedDeltaTime);

        // Handle dash logic
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
            // Perform dash move
            rb.MovePosition(rb.position + movement.normalized * dashAmount);
            isDash = false;

            // Set dash cooldown
            lastDashTime = Time.time;
            canDash = false;

            StartCoroutine(DashCooldown());
        }
    }

    void UpdateFacingDirection()
    {
        // Update facing direction to match current movement
        facingDirection = movement.normalized;

        // Move the attack point to the new facing direction
        RepositionAttackPoint();
    }

    void RepositionAttackPoint()
    {
        // Adjust attack point distance based on direction (horizontal or vertical)
        Vector2 adjustedDistance = new Vector2(
            facingDirection.x * horizontalAttackDistance, // Shorter distance for horizontal movement
            facingDirection.y * verticalAttackDistance     // Longer distance for vertical movement
        );

        // Set the attack point's position based on the adjusted distance
        attackPoint.localPosition = adjustedDistance;
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
