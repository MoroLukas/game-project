using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 6;
    public int currentHP;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged; // current, max

    void Start() => currentHP = maxHP;

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        OnHealthChanged?.Invoke(currentHP, maxHP);
        if (currentHP <= 0) Die();
    }

    public void AddLive(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void AddMaxHP(int amount)
    {
        maxHP += amount;
        currentHP += amount; // o tienilo invariato, scelta di design
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}