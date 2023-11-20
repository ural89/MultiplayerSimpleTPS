using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBar;
    private void Start()
    {
        health.OnDie += Health_OnDie;
    }

    private void Health_OnDie()
    {
        healthBar.fillAmount = 0;
    }

    private void Update()
    {
        if (!health.IsDead)
            healthBar.fillAmount = health.GetHealthNormilized;
    }
}
