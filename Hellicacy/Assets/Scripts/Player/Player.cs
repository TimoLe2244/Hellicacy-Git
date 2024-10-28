using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HealthBar healthBar;
    private EnergyBar energyBar;
    public bool canCastUlt;

    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        energyBar = FindObjectOfType<EnergyBar>();

        UpdateBars();
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
}

