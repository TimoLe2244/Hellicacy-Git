using UnityEngine;

public class EquipSprite : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer;
    public Sprite newSprite;
    public KeyCode interactKey = KeyCode.E;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            Equip();
        }
    }

    public void Equip()
    {
        if (playerSpriteRenderer != null && newSprite != null)
        {
            playerSpriteRenderer.sprite = newSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
