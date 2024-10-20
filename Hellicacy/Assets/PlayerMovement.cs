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

    private bool canDash = true;
    private float lastDashTime; 
    public float currentSpd;
    public Rigidbody2D rb;
    Vector2 movement;

    // Initially set to -1, since the sprite faces left by default
    public int facingDirection = -1; 

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Trigger dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            isDash = true;
        }
    }

    private void FixedUpdate()
    {
        // Update speed based on sprinting
        if (TestIfSprinting())
        {
            currentSpd = sprintSpd;
        }
        else
        {
            currentSpd = walkSpd;
        }

        // Flip the player based on movement direction
        if ((movement.x > 0 && facingDirection < 0) || (movement.x < 0 && facingDirection > 0))
        {
            Flip();
        }

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
        if (!canSprint) { return false; }

        return Input.GetKey(KeyCode.LeftShift);
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

    void Flip()
    {
        // Reverse the facing direction
        facingDirection *= -1;

        // Flip the character's sprite by inverting the x-axis scale
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Only flip the x-axis, as the sprite originally faces left
        transform.localScale = localScale;
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
