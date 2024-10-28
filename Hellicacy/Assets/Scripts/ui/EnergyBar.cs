using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image energyBarFill;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void UpdateEnergyBar(int currentEnergy, int maxEnergy)
    {
        float fillAmount = (float)currentEnergy / maxEnergy;
        energyBarFill.fillAmount = fillAmount;
    }
}
