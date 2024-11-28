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

    public GameObject deathPanel;
    public GameObject endPanel;

    void Start()
    {
        maxHealth = ResourceManager.Instance.maxPlayerHP;
        arrows = ResourceManager.Instance.playerArrowsToGive;

        currentHealth = LevelController.playerHp == 0 ? maxHealth : Mathf.Min(maxHealth, LevelController.playerHp + 1.0f);
        LevelController.playerHp = currentHealth;
    }

    public int GetCurrentArrows()
    {
        return arrows;
    }

    public void TakeArrow()
    {
        if (arrows > 0) arrows -= 1;
        LevelController.playerArrowShootEvent.Invoke();
    }

    public void AddHealth(float amount)
    {
        if (currentHealth + amount > maxHealth) currentHealth = maxHealth;
        else currentHealth += amount;
        LevelController.playerHpEvent.Invoke();
    }

    void Die()
    {
        ResetItems();
        deathPanel.SetActive(true);
        Time.timeScale = 0.01f;
    }

    public void TakeDamage(float damage, GameObject sender = null)
    {
        if (gameObject == sender) return;
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            LevelController.playerHpEvent.Invoke();
        }
        else
        {
            currentHealth = 0;
            LevelController.playerHpEvent.Invoke();
            Die();
        }
    }

    void ResetItems()
    {
        LevelController.playerHasBow = false;
        LevelController.playerHasSword = false;
        LevelController.playerHasHelmet = false;
    }

    public void ShowEndPanel()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0.01f;
    }
}
