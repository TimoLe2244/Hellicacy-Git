using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointManager : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] float horizontalAttackDistance = 1.2f;
    [SerializeField] float verticalAttackDistance = 1.5f;

    private Vector2 facingDirection = new Vector2(-1, 0); // Default to left

    public void UpdateAttackPoint(Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            facingDirection = movementDirection.normalized;
            RepositionAttackPoint();
        }
    }

    private void RepositionAttackPoint()
    {
        Vector2 adjustedDistance = new Vector2(
            facingDirection.x * horizontalAttackDistance,
            facingDirection.y * verticalAttackDistance
        );

        attackPoint.localPosition = adjustedDistance;
    }
}
