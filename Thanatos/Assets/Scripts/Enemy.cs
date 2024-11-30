using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject heartPrefab;
    public GameObject helmetPrefab;
    protected bool isBoss = false;

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
        if (isBoss)
        {
            LevelController.bossDeathEvent.Invoke();
            Destroy(gameObject);
        }
        else
        {
            LevelController.enemyDeathEvent.Invoke();
            SpawnRandomItem();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, GameObject sender = null)
    {
        if (gameObject == sender) return;
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            ShowDamage();
        }
        else Die();
    }

    private void SpawnRandomItem()
    {
        int rnd = Random.Range(0, 100);
        if (rnd < 6)
        {
            if (helmetPrefab == null) return;
            if (!LevelController.playerHasHelmet)
                Instantiate(helmetPrefab, transform.position, Quaternion.identity);
        }
        else if (rnd < 40)
        {
            if (heartPrefab == null) return;
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
        }
    }

    protected virtual void ShowDamage()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
        Invoke("StopShowDamage", 0.1f);
    }

    private void StopShowDamage()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
