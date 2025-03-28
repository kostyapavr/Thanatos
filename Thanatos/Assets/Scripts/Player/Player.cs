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

    public GameObject slowEffectSprite;
    public GameObject ambrosiaEffectSprite;
    public GameObject miasmaEffectSprite;

    public Sprite defaultArmorSprite;
    public Sprite goldenArmorSprite;

    public GameObject lavaBoots;

    public Bow bow;
    private int selectedWeapon = 0;
    private PlayerCombat sword;
    private bool godMode = false;

    private float hpAddWhenNewLevel = 2f;
    private float hpAddWhenNewLevel_hard = 1f;

    [HideInInspector]
    public bool is_enemySlowEffect = false;
    private float enemySlowEffectDuration = 5.0f;
    private float enemySlowEffectTime = 0f;

    private bool miasmaEffect = false;
    private bool ambrosiaEffect = false;
    private bool goldenArmorEffect = false;
    private bool hasArmor = false;
    private bool hasLavaBoots = false;

    void Start()
    {
        maxHealth = ResourceManager.Instance.MaxPlayerHP;
        arrows = ResourceManager.Instance.PlayerArrowsToGive;
        if (!LevelController.playerHasSword && LevelController.playerHasBow) arrows += 10;

        currentHealth = LevelController.playerHp == 0 ? maxHealth : Mathf.Min(maxHealth, LevelController.playerHp + (LevelController.isNormalDifficulty ? hpAddWhenNewLevel : hpAddWhenNewLevel_hard));
        LevelController.playerHp = currentHealth;

        CheckPickup();
        LevelController.playerPickupItemEvent.AddListener(CheckPickup);

        godMode = LevelController.playerIsGod;

        sword = GetComponent<PlayerCombat>();

        if (LevelController.playerHasBow || LevelController.playerHasSword)
            SelectWeapon(LevelController.currentPlayerWeapon);

        if (LevelController.playerHasArmor) EquipArmor();
        if (LevelController.playerHasLavaBoots) EquipLavaBoots();
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

        if (is_enemySlowEffect)
        {
            if (enemySlowEffectTime > 0) enemySlowEffectTime -= Time.deltaTime;
            else
            {
                is_enemySlowEffect = false;
                GetComponent<PlayerMovement>().ReturnToNormalSpeed();
                slowEffectSprite.SetActive(false);
            }
        }
    }

    public void ApplyEnemySlowEffect()
    {
        is_enemySlowEffect = true;
        enemySlowEffectTime = enemySlowEffectDuration;
        slowEffectSprite.SetActive(true);
        GetComponent<PlayerMovement>().SlowDown_Effect();
    }

    public void SwitchWeapon(int n)
    {
        if (n == 1 && LevelController.playerHasBow) SelectBow();
        else if (n == 0 && LevelController.playerHasSword) SelectSword();
    }

    public void SelectWeapon(IPickupableWeapon curWeapon)
    {
        if (curWeapon.Name.Contains("Sword") && LevelController.playerHasSword) SelectSword();
        else if (curWeapon.Name.Contains("Bow") && LevelController.playerHasBow) SelectBow();
    }

    private void SelectSword()
    {
        bow.HideBow();
        sword.ShowSword();
        selectedWeapon = 1;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Sword"));
    }

    private void SelectBow()
    {
        bow.ShowBow();
        sword.HideSword();
        selectedWeapon = 0;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Bow"));
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

    public void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing)
    {
        if (gameObject == sender || godMode) return;

        if (miasmaEffect) damage += 0.5f;
        if (ambrosiaEffect)
        {
            RemoveAmbrosiaEffect();
            return;
        }
        if (goldenArmorEffect)
        {
            goldenArmorEffect = false;
            return;
        }
        if (LevelController.playerHasArmor) damage -= 0.5f;

        if (currentHealth - damage > 0)
        { 
            if (damageEffect == DamageEffects.SlowDown) ApplyEnemySlowEffect();
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
        LevelController.playerHasArmor = false;
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

        if (!hasArmor && LevelController.playerHasArmor) EquipArmor();
        if (!hasLavaBoots && LevelController.playerHasLavaBoots) EquipLavaBoots();

        switch (LevelController.playerBonusType)
        {
            case BonusTypes.None: break;
            case BonusTypes.Moli: AddHealth(0.5f); LevelController.playerBonusType = BonusTypes.None; break;
            case BonusTypes.Baytulus: AddHealth(2f); LevelController.playerBonusType = BonusTypes.None; break;
            case BonusTypes.Cornucopia: AddHealth(1f); LevelController.playerBonusType = BonusTypes.None; break;
            case BonusTypes.Miasma: ApplyMiasmaEffect(); break;
            case BonusTypes.Ambrosia: ApplyAmbrosiaEffect(); break;
            case BonusTypes.Garnet: ApplyGarnetEffect(); LevelController.playerBonusType = BonusTypes.None; break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ExitPortal")
        {
            if (miasmaEffect) RemoveMiasmaEffect();
            if (ambrosiaEffect) RemoveAmbrosiaEffect();
        }
        if (collision.name == "Boss1") TakeDamage(0.5f, collision.gameObject);
    }

    private void ApplyAmbrosiaEffect()
    {
        ambrosiaEffect = true;
        ambrosiaEffectSprite.SetActive(true);
    }

    private void RemoveAmbrosiaEffect()
    {
        ambrosiaEffect = false;
        ambrosiaEffectSprite.SetActive(false);
        LevelController.playerBonusType = BonusTypes.None;
    }

    private void ApplyMiasmaEffect()
    {
        miasmaEffect = true;
        miasmaEffectSprite.SetActive(true);
    }

    private void RemoveMiasmaEffect()
    {
        miasmaEffect = false;
        miasmaEffectSprite.SetActive(false);
        LevelController.playerBonusType = BonusTypes.None;
    }

    private void ApplyGarnetEffect()
    {
        int rnd = Random.Range(0, 100);
        if (rnd < 50) AddHealth(5);
        else TakeDamage(10);
    }

    public void EquipArmor()
    {
        GetComponent<SpriteRenderer>().sprite = goldenArmorSprite;
        goldenArmorEffect = true;
        hasArmor = true;
    }

    public void RemoveArmor()
    {
        GetComponent<SpriteRenderer>().sprite = defaultArmorSprite;
        goldenArmorEffect = false;
        hasArmor = false;
    }

    public void EquipLavaBoots()
    {
        lavaBoots.SetActive(true);
        hasLavaBoots = true;
    }

    public void RemoveLavaBoots()
    {
        lavaBoots.SetActive(false);
        hasLavaBoots = false;
    }
}
