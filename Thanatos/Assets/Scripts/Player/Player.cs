using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject armorSprite;
    public GameObject achillesHelmetSprite;
    public GameObject achillesArmorSprite;

    public GameObject pauseMenu;

    public GameObject slowEffectSprite;
    public GameObject ambrosiaEffectSprite;
    public GameObject miasmaEffectSprite;

    public Sprite goldenArmorSprite;

    public GameObject lavaBoots;

    public Bow bow;
    private PlayerCombat sword;
    public Shield shield;
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
    private bool achillesArmorEffect = false;
    private bool achillesHelmetEffect = false;

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

        if (LevelController.playerHasBow || LevelController.playerHasSword || LevelController.playerHasShield)
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
            BookScript bs = FindObjectOfType<BookScript>();
            if (bs == null || !bs.isOpen)
            {
                if (!pauseMenu.GetComponent<PauseMenu>().isInSettings)
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
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(LevelController.playerWeapons.Where(x => x.Name.Contains("Bow")).FirstOrDefault());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(LevelController.playerWeapons.Where(x => x.Name.Contains("Sword")).FirstOrDefault());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectWeapon(LevelController.playerWeapons.Where(x => x.Name.Contains("Shield")).FirstOrDefault());
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject.FindGameObjectWithTag("ExitPortal").GetComponent<Portal>().LoadNextLevel();
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
        if (n == 1 && LevelController.playerHasBow && !sword.isAttacking && !LevelController.playerShieldActive) SelectBow();
        else if (n == 0 && LevelController.playerHasSword && !bow.isCharging && !LevelController.playerShieldActive) SelectSword();
        else if (n == 2 && LevelController.playerHasShield && !bow.isCharging && !sword.isAttacking) SelectShield();
    }

    public void SelectWeapon(IPickupableWeapon weapon)
    {
        if (weapon == null) return;
        if (weapon.Name.Contains("Sword") && LevelController.playerHasSword && !bow.isCharging && !LevelController.playerShieldActive) SelectSword();
        else if (weapon.Name.Contains("Bow") && (LevelController.playerHasBow || LevelController.playerHasFireBow) && !sword.isAttacking && !LevelController.playerShieldActive) SelectBow();
        else if (weapon.Name.Contains("Shield") && LevelController.playerHasShield && !bow.isCharging && !sword.isAttacking) SelectShield();
    }

    private void SelectSword()
    {
        bow.HideBow();
        sword.ShowSword();
        shield.HideShield();
        //selectedWeapon = 1;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Sword"));
    }

    private void SelectBow()
    {
        bow.ShowBow();
        sword.HideSword();
        shield.HideShield();
        //selectedWeapon = 0;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Bow"));
    }

    private void SelectShield()
    {
        bow.HideBow();
        sword.HideSword();
        shield.ShowShield();
        //selectedWeapon = 0;
        LevelController.currentPlayerWeapon = LevelController.playerWeapons.Find(x => x.Name.Contains("Shield"));
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

        if (LevelController.playerShieldActive) return;

        if (miasmaEffect) damage += 0.5f;
        if (ambrosiaEffect)
        {
            RemoveAmbrosiaEffect();
            return;
        }
        if (goldenArmorEffect)
        {
            goldenArmorEffect = false;
            ShowArmorTakeDamage();
            return;
        }
        if (achillesArmorEffect && sender != null && sender.GetComponent<MeleeEnemy>())
        {
            damage = 0.25f;
            ShowArmorTakeDamage();
        }

        if (LevelController.playerHasArmor)
        {
            damage /= 2f;
            ShowArmorTakeDamage();
        }
        

        if (currentHealth - damage > 0)
        { 
            if (damageEffect == DamageEffects.SlowDown) ApplyEnemySlowEffect();
            currentHealth -= damage;
            LevelController.playerHpEvent.Invoke();
            ShowArmorTakeDamage();
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
        LevelController.playerHasFireBow = false;
        LevelController.playerHasAchillesArmor = false;
        LevelController.playerHasAchillesHelmet = false;
        LevelController.playerHasShield = false;
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
        if (!hasArmor && LevelController.playerHasAchillesArmor) EquipAchillesArmor();
        if (!achillesHelmetEffect && LevelController.playerHasAchillesHelmet) EquipAchillesHelmet();
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

    private void ShowArmorTakeDamage()
    {
        helmetSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        achillesArmorSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        achillesHelmetSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        armorSprite.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.5f, 0.5f, 1f);
        Invoke("StopShowDamage", 0.1f);
    }

    private void StopShowDamage()
    {
        helmetSprite.GetComponent<SpriteRenderer>().color = Color.white;
        achillesArmorSprite.GetComponent<SpriteRenderer>().color = Color.white;
        achillesHelmetSprite.GetComponent<SpriteRenderer>().color = Color.white;
        armorSprite.GetComponent<SpriteRenderer>().color = Color.white;
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
        armorSprite.SetActive(true);
        goldenArmorEffect = true;
        hasArmor = true;
    }

    public void RemoveArmor()
    {
        armorSprite.SetActive(false);
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

    public void EquipAchillesArmor()
    {
        achillesArmorSprite.SetActive(true);
        achillesArmorEffect = true;
        hasArmor = true;
    }

    public void RemoveAchillesArmor()
    {
        achillesArmorSprite.SetActive(false);
        achillesArmorEffect = false;
        hasArmor = false;
    }

    public void EquipAchillesHelmet()
    {
        achillesHelmetSprite.SetActive(true);
        achillesHelmetEffect = true;
    }

    public void RemoveAchillesHelmet()
    {
        achillesHelmetSprite.SetActive(false);
        achillesHelmetEffect = false;
    }
}
