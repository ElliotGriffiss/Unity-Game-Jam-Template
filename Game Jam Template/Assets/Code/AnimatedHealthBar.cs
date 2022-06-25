using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedHealthBar : MonoBehaviour
{
    [SerializeField] protected Image HealthBar;
    [SerializeField] protected Text Healthtext;
    [Space]
    [SerializeField] protected float MoveDownDuration;
    [SerializeField] protected Color NormalColor;
    [SerializeField] protected Color TakeDamageColor;
    [SerializeField] protected Color NormalTextColor;
    [SerializeField] protected Color TakeDamageTextColor;

    private float CurrentHealth;
    private float CurrentMaxHealth;
    private float StartHealth;
    private float StartTime = 0;

    public void SetHealth(bool Immediate, float health, float maxHealth)
    {
        if (Immediate)
        {
            Healthtext.text = "Health: " + Mathf.RoundToInt(health);
            HealthBar.fillAmount = health / maxHealth;
            StartHealth = HealthBar.fillAmount;
        }
        else if (CurrentHealth != health)
        {
            StartHealth = HealthBar.fillAmount;
            StartTime = 0;
        }

        CurrentHealth = health;
        CurrentMaxHealth = maxHealth;

        HealthBar.color = NormalColor;
        Healthtext.color = NormalTextColor;
        Healthtext.text = "Health: " + Mathf.RoundToInt(health);
    }

    public void Update()
    {
        if (StartTime < MoveDownDuration)
        {
            Healthtext.color = TakeDamageTextColor;
            HealthBar.color = TakeDamageColor;

            HealthBar.fillAmount = Mathf.Lerp(StartHealth, CurrentHealth / CurrentMaxHealth, StartTime / MoveDownDuration);
            StartTime += Time.deltaTime;
        }
        else
        {
            HealthBar.color = NormalColor;
            Healthtext.color = NormalTextColor;
        }
    }
}
