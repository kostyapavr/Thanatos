using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 3;
    private float currentHealth = 3;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
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
        Debug.Log($"Added {amount} HP. Current HP: {currentHealth}");
    }

    void Die()
    {
        Debug.Log("Player died");
    }


}
