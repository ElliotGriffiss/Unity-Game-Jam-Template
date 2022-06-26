using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomDataTypes;

public class AnimatedHealthBar : MonoBehaviour
{
    [SerializeField] protected Image HealthBar;
    [Space]
    [SerializeField] protected float MoveDownDuration;
    [SerializeField] protected Color NormalColor;
    [SerializeField] protected Color TakeDamageColor;
    [SerializeField] protected Color LightDamageColor;
    [SerializeField] protected Color HealingColor;


    private float CurrentHealth;
    private float CurrentMaxHealth;
    private float StartHealth;
    private float StartTime = 0;
    private DamageType damageType;

    public void SetHealth(DamageType type, float health, float maxHealth)
    {

        if (damageType == DamageType.LightDamage && type == DamageType.Immediate && damageType != DamageType.Damage)
        {
            HealthBar.color = NormalColor;
        }
        else
        {
            switch (type)
            {
                case DamageType.Immediate:
                    StartTime = MoveDownDuration;
                    HealthBar.fillAmount = health / maxHealth;
                    HealthBar.color = NormalColor;
                    break;
                case DamageType.Damage:
                    StartHealth = HealthBar.fillAmount;
                    StartTime = 0;
                    HealthBar.color = TakeDamageColor;
                    break;
                case DamageType.LightDamage:
                    StartTime = MoveDownDuration;
                    HealthBar.fillAmount = health / maxHealth;
                    StartHealth = HealthBar.fillAmount;
                    HealthBar.color = LightDamageColor;
                    break;
                case DamageType.Healing:
                    StartTime = MoveDownDuration;
                    HealthBar.fillAmount = health / maxHealth;
                    StartHealth = HealthBar.fillAmount;
                    HealthBar.color = HealingColor;
                    break;
            }
        }

        CurrentHealth = health;
        CurrentMaxHealth = maxHealth;
    }

    public void Update()
    {
        if (StartTime < MoveDownDuration)
        {
            HealthBar.color = TakeDamageColor;

            HealthBar.fillAmount = Mathf.Lerp(StartHealth, CurrentHealth / CurrentMaxHealth, StartTime / MoveDownDuration);
            StartTime += Time.deltaTime;

            if (StartTime < MoveDownDuration)
            {
                HealthBar.color = NormalColor;
            }
        }
    }
}
