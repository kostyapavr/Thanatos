using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int itemDropChance;
    public GameObject[] dropItems;
    public GameObject helmetPrefab;
    public bool isBoss;
    private bool itemSpawned = false;

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

    public virtual void Die()
    {
        LevelController.enemyDeathEvent.Invoke();
        SpawnRandomItem();
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing)
    {
        if (gameObject == sender) return;
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            ShowDamage();
        }
        else Die();
    }

    protected virtual void SpawnRandomItem()
    {
        int rnd = Random.Range(0, 100);
        if (rnd <= 1 && !isBoss)
        {
            if (helmetPrefab == null) return;
            if (!LevelController.playerHasHelmet)
                Instantiate(helmetPrefab, transform.position, Quaternion.identity);
            return;
        }

        if (rnd <= itemDropChance && !itemSpawned)
        {
            int rndInd = Random.Range(0, dropItems.Length);
            if (dropItems == null || dropItems.Length == 0) return;
            if (dropItems[rndInd].GetComponent<BonusPickup>().bonusType == BonusTypes.AriadneThread && LevelController.isBossLevel) return;
            Instantiate(dropItems[rndInd], transform.position, Quaternion.identity);
            itemSpawned = true;
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
