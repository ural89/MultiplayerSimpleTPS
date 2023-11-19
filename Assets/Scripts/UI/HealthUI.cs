using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBar;
    private void Update()
    {
        if (!health.IsDead)
            healthBar.fillAmount = health.GetHealthNormilized;
    }
}
