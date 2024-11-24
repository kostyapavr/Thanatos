using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int arrows = 5;

    private float _currentHealth;
    private float _maxHealth;
    public float currentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float maxHealth { get => _maxHealth; set => _maxHealth = value; }

    void Start()
    {
        maxHealth = ResourceManager.Instance.maxPlayerHP;
        arrows = ResourceManager.Instance.playerArrowsToGive;
        currentHealth = maxHealth;
        Debug.Log(currentHealth);
        Debug.Log(maxHealth);
    }

    public int GetCurrentArrows()
    {
        return arrows;
    }

    public void TakeArrow()
    {
        if (arrows > 0) arrows -= 1;
    }

    public void AddHealth(float amount)
    {
        if (currentHealth + amount > maxHealth) currentHealth = maxHealth;
        else currentHealth += amount;
        LevelController.playerHpEvent.Invoke();
        Debug.Log($"Added {amount} HP. Current HP: {currentHealth}");
    }

    void Die()
    {
        Debug.Log("Player died");
    }

    public void TakeDamage(float damage, GameObject sender = null)
    {
        if (gameObject == sender) return;
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            LevelController.playerHpEvent.Invoke();
            Debug.Log($"Damage: {damage} HP. Current HP: {currentHealth}");
        }
        else
        {
            currentHealth = 0;
            Die();
        }
    }
}
