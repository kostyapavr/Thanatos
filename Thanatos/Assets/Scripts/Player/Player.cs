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
    public GameObject helmetSprite;

    public GameObject pauseMenu;

    public Bow bow;
    private int selectedWeapon = 0;
    private PlayerCombat sword;
    private bool godMode = false;

    void Start()
    {
        maxHealth = ResourceManager.Instance.MaxPlayerHP;
        arrows = ResourceManager.Instance.PlayerArrowsToGive;
        if (!LevelController.playerHasSword && LevelController.playerHasBow) arrows += 10;

        currentHealth = LevelController.playerHp == 0 ? maxHealth : Mathf.Min(maxHealth, LevelController.playerHp + 1.0f);
        LevelController.playerHp = currentHealth;

        CheckPickup();
        LevelController.playerPickupItemEvent.AddListener(CheckPickup);

        godMode = LevelController.playerIsGod;

        sword = GetComponent<PlayerCombat>();

        if (LevelController.playerHasBow || LevelController.playerHasSword)
            SelectWeapon(LevelController.playerSelectedWeapon);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            LevelController.playerIsGod = godMode;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(selectedWeapon);
        }
    }

    public void SwitchWeapon(int n)
    {
        if (n == 1 && LevelController.playerHasBow) SelectBow();
        else if (n == 0 && LevelController.playerHasSword) SelectSword();
    }

    public void SelectWeapon(int n)
    {
        if (n == 1 && LevelController.playerHasSword) SelectSword();
        else if (n == 0 && LevelController.playerHasBow) SelectBow();
    }

    private void SelectSword()
    {
        bow.HideBow();
        sword.ShowSword();
        selectedWeapon = 1;
        LevelController.playerSelectedWeapon = 1;
    }

    private void SelectBow()
    {
        bow.ShowBow();
        sword.HideSword();
        selectedWeapon = 0;
        LevelController.playerSelectedWeapon = 0;
    }

    public int GetCurrentArrows()
    {
        return arrows;
    }

    public void TakeArrow()
    {
        if (arrows > 0 && !godMode) arrows -= 1;
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
        Time.timeScale = 0;
    }

    public void TakeDamage(float damage, GameObject sender = null)
    {
        if (gameObject == sender || godMode) return;
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
        Time.timeScale = 0;
    }

    void CheckPickup()
    {
        helmetSprite.SetActive(LevelController.playerHasHelmet);
        helmetSprite.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Boss1") TakeDamage(0.5f, collision.gameObject);
    }
}
