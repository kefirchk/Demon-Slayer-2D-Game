using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image healthBar;
    public float curHealth;
    public float maxHealth = 100f;
    void Start()
    {
        healthBar = GetComponent<Image>();
        curHealth = maxHealth;
    }

    void Update()
    {
        curHealth = Hero.Instance.lives;
        healthBar.fillAmount = curHealth / maxHealth;
    }
}
