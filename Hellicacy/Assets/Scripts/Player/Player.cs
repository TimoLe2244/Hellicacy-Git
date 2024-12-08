using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthBar healthBar;
    private EnergyBar energyBar;
    private LivesUI livesUI;

    public bool canCastUlt;

    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        energyBar = FindObjectOfType<EnergyBar>();
        livesUI = FindObjectOfType<LivesUI>();

        UpdateBars();
        UpdateLivesUI();
    }

    public void ChangeHealth(int amount)
    {
        GameManager.Instance.ChangeHealth(amount);
        Debug.Log($"Player Health Changed: Amount = {amount}, Current Health = {GameManager.Instance.currentHealth}");
        UpdateBars();
    }

    public void ChangeEnergy(int amount)
    {
        GameManager.Instance.ChangeEnergy(amount);
        Debug.Log($"Player Energy Changed: Amount = {amount}, Current Energy = {GameManager.Instance.currentEnergy}");
        UpdateBars();

        canCastUlt = GameManager.Instance.currentEnergy >= 100;
    }

    public void ChangeLives(int amount)
    {
        GameManager.Instance.ChangeLives(amount);
        Debug.Log($"Player died: Lives {amount}");
        UpdateLivesUI();
    }

    private void UpdateBars()
    {
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(GameManager.Instance.currentHealth, GameManager.Instance.maxHealth);
        }

        if (energyBar != null)
        {
            energyBar.UpdateEnergyBar(GameManager.Instance.currentEnergy, GameManager.Instance.maxEnergy);
        }
    }

    private void UpdateLivesUI()
    {
        if (livesUI != null)
        {
            livesUI.UpdateLivesUI(GameManager.Instance.currentLives);
        }
    }

        public void TakeKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}


