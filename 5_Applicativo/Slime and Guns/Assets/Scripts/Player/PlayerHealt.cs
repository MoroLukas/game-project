using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    

    public int maxHP = 6;
    public int currentHP;

    public float invincibilityTime = 1f;
    public float flashInterval = 0.05f;

    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHP = Mathf.Max(0, currentHP - amount);
        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
            Die();
        else
            StartCoroutine(Flash());
    }

    public void AddLive(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void AddMaxHP(int amount)
    {
        maxHP += amount;
        currentHP += amount;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    private IEnumerator Flash()
    {
        isInvincible = true;
        float timer = 0f;

        while (timer < invincibilityTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}