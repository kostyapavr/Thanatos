using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject heartPrefab;

    private float _currentHealth;
    private float _maxHealth;
    public float currentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }
    public float maxHealth 
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    public virtual void Start()
    {
        maxHealth = 5.0f;
        currentHealth = maxHealth;
    }

    public virtual void FixedUpdate()
    {

    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void Die()
    {
        LevelController.enemyDeathEvent.Invoke();
        Instantiate(heartPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, GameObject sender = null)
    {
        if (gameObject == sender) return;
        if (currentHealth - damage > 0) currentHealth -= damage;
        else Die();
    }
}
