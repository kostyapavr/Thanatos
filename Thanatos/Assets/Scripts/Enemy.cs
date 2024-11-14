using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth = 100;

    public GameObject arrowPref;
    public Transform firePoint;
    public float shootInterval = 2;

    void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("Shoot", shootInterval, shootInterval);
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(arrowPref, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = (FindObjectOfType<Player>().transform.position - firePoint.position).normalized * 10;
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

    void Die()
    {
        LevelController.enemyDeathEvent.Invoke();
        Destroy(gameObject);
    }
}
