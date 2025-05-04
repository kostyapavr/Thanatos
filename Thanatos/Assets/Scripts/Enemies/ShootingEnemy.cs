using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ShootingEnemy : Enemy
{
    public float minHealth;
    public new float maxHealth;

    public float shootIntervalMin;
    public float shootIntervalMax;
    
    public int ammoSpeedMin;
    public int ammoSpeedMax;

    public int ammoSpread;

    public GameObject ammoPrefab;

    public Sprite normalSprite;
    public Sprite aimSprite;

    public float shootingRange;
    public LayerMask PlayerMask;

    public bool shootAhead;
    public DamageEffects playerDamageEffect;
    public GameObject fireEffectParticles;
    public GameObject confusionEffectParticles;
    public Transform confusionEffectSpot;

    protected Player player;
    protected SpriteRenderer spriteRenderer;

    protected float health;
    protected float shootInterval;
    protected int ammoSpeed;

    private bool stopOneShot = false;
    [HideInInspector] public bool freeze = false;
    public bool immuneToFire = false;
    public bool immuneToConfusion = false;
    private bool isOnFire = false;
    private bool isConfused = false;
    public bool shootScatterArrow = false;

    public override void Start()
    {
        RandomiseProperties();

        currentHealth = health;
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("Aim", shootInterval-0.75f, shootInterval-0.75f);
        InvokeRepeating("Shoot", shootInterval+1, shootInterval);

        if (immuneToFire && !isBoss)
        {
            Instantiate(fireEffectParticles, transform);
        }
    }

    protected bool PlayerInSight()
    {
        if (isConfused) return false;
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= shootingRange)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position);
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, shootingRange, PlayerMask);
            
            if (hit.collider.GetComponent<Player>()) return true;
        }
        return false;
    }

    protected void Aim()
    {
        if (aimSprite == null || freeze || isConfused) return;
        spriteRenderer.sprite = aimSprite;
    }

    protected virtual void Shoot()
    {
        spriteRenderer.sprite = normalSprite;
        if (!PlayerInSight()) return;
        if (stopOneShot)
        {
            stopOneShot = false;
            return;
        }
        if (freeze)
        {
            freeze = false;
            return;
        }

        Vector2 diff = player.transform.position - transform.position;
        if (shootAhead) diff = ((Vector2)player.transform.position + player.GetComponent<Rigidbody2D>().velocity*0.3f) - (Vector2)transform.position;

        if (shootScatterArrow)
        {
            ShootScatterArrow(diff);
            return;
        }

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float spreadAngle = angle + Random.Range(LevelController.playerHasAchillesHelmet ? -ammoSpread*2 : -ammoSpread, LevelController.playerHasAchillesHelmet ? ammoSpread*2 + 1 : ammoSpread + 1);
        Vector2 direction = Quaternion.AngleAxis(spreadAngle - angle, Vector3.forward) * diff.normalized;

        if (!spriteRenderer.flipX && direction.x < 0)
            spriteRenderer.flipX = true;
        else if (spriteRenderer.flipX && direction.x > 0)
            spriteRenderer.flipX = false;

        GameObject arrow = Instantiate(ammoPrefab, transform.position, Quaternion.Euler(0, 0, spreadAngle + 270));
        arrow.GetComponent<Arrow>().OwnerType = "Enemy";
        arrow.GetComponent<Arrow>().OwnerID = gameObject.GetInstanceID();
        arrow.GetComponent<Arrow>().damageEffect = playerDamageEffect;
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * ammoSpeed + GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
    }

    void ShootScatterArrow(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        List<float> spreadAngles = new List<float> { angle - 12, angle, angle + 12 };

        for (int i = 0; i < 3; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(spreadAngles[i] - angle, Vector3.forward) * dir.normalized;
            Rigidbody2D rb = Instantiate(ammoPrefab, transform.position, Quaternion.Euler(0, 0, spreadAngles[i] + 270)).GetComponent<Rigidbody2D>();
            rb.GetComponent<Arrow>().OwnerType = "Enemy";
            rb.GetComponent<Arrow>().OwnerID = gameObject.GetInstanceID();
            rb.GetComponent<Arrow>().damageEffect = DamageEffects.Nothing;
            rb.GetComponent<Arrow>().Damage = 0.75f;
            rb.AddForce(direction * ammoSpeed + (Vector3)GetComponent<Rigidbody2D>().velocity, ForceMode2D.Impulse);
        }
    }

    protected virtual void RandomiseProperties()
    {
        health = Random.Range(minHealth, maxHealth);
        shootInterval = Random.Range(shootIntervalMin, shootIntervalMax);
        ammoSpeed = Random.Range(ammoSpeedMin, ammoSpeedMax);
    }

    public override void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing)
    {
        base.TakeDamage(damage, sender, damageEffect);
        if (damageEffect == DamageEffects.StopOneShot) stopOneShot = true;
        if (damageEffect == DamageEffects.FreezeInPlace) freeze = true;
        if (damageEffect == DamageEffects.SetOnFire && !immuneToFire && !isOnFire)
        {
            Instantiate(fireEffectParticles, transform);
            isOnFire = true;
            stopOneShot = true;
            Invoke("FireDamage", 1f);
            Invoke("FireDamage", 2f);
            Invoke("FireDamage", 3f);
            Invoke("FireDamage", 4f);
            Invoke("FireDamage", 5f);
            Invoke("StopOnFire", 5f);
        }
        if (damageEffect == DamageEffects.Confusion && !immuneToConfusion && !isConfused)
        {
            Instantiate(confusionEffectParticles, confusionEffectSpot);
            isConfused = true;
            Invoke("StopConfusion", 3f);
        }
    }

    void FireDamage()
    {
        TakeDamage(0.2f, null, DamageEffects.Bleed);
    }

    void StopOnFire()
    {
        isOnFire = false;
    }

    void StopConfusion()
    {
        isConfused = false;
    }
}
