using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 3;
    private float currentHealth = 3;
    private int arrows = 5;

    void Start()
    {
        maxHealth = ResourceManager.Instance.maxPlayerHP;
        arrows = ResourceManager.Instance.playerArrowsToGive;
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetCurrentArrows()
    {
        return arrows;
    }

    public void TakeArrow()
    {
        if (arrows > 0) arrows -= 1;
    }

    public void TakeDamage(float damage)
    {
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
}
