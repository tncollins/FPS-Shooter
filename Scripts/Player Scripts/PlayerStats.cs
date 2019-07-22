using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image healthBar, staminaBar;

    public void DisplayHealthStats(float healthValue)
    {
        healthValue /= 100f;
        healthBar.fillAmount = healthValue;
    }

    public void DisplayStaminaStats(float staminaValue)
    {
        staminaValue /= 100f;
        staminaBar.fillAmount = staminaValue;
    }
}
