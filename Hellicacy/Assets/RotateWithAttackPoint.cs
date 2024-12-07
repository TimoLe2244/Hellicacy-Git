using UnityEngine;

public class RotateWithAttackPoint : MonoBehaviour
{
    public Transform attackPoint; // Reference to the attack point

    private void Update()
    {
        // Make the VFX follow the attack point's position
        transform.position = attackPoint.position;

        // Make the VFX follow the attack point's rotation
        transform.rotation = attackPoint.rotation;
    }
}
 