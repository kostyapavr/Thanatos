using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int maxHealth = 100;
    protected int currentHealth = 100;
    public GameObject heartPrefab;

    public virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void FixedUpdate()
    {

    }

    public void TakeDamage(int damage)
    {   
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void Die()
    {
        LevelController.enemyDeathEvent.Invoke();
        Instantiate(heartPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
