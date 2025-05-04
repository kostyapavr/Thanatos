using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MeleeEnemy : Enemy
{
    public float minHealth;
    public new float maxHealth;

    public float attackIntervalMin;
    public float attackIntervalMax;

    public float damage;

    public float moveSpeedMin;
    public float moveSpeedMax;

    public float obstacleDetectSize;

    [HideInInspector] public Transform player;

    private float detectionDistance = 6f;

    public float attackRange;
    private float chaseRange = 60f;

    private float lastAttackTime;

    private float health;
    private float attackInterval;
    private float moveSpeed;

    private int obstacleMask;

    public Sprite firstSprite;
    public Sprite secondSprite;
    private int animInd = 0;

    public float walkAnimTime;

    public DamageEffects dmgEffect;
    public GameObject fireEffectParticles;
    public GameObject confusionEffectParticles;
    public Transform confusionEffectSpot;
    public bool immuneToConfusion = false;
    private bool isConfused = false;

    private SpriteRenderer spriteRenderer;

    private bool freeze = false;
    private float freezeTimer = 1.5f;
    private float frzTmr = 0.0f;

    private bool isOnFire = false;

    public bool canTurnTransparent = false;
    public int transparentChance = 0;
    private bool isTransparent = false;

    public override void Start()
    {
        RandomiseProperties();

        currentHealth = health;
        //InvokeRepeating("Attack", attackInterval, attackInterval);
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        obstacleMask = LayerMask.GetMask("Obstacle");
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + attackInterval)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (var hit in hitPlayer)
            {
                if (hit.CompareTag("Player"))
                {
                    Player player = hit.GetComponent<Player>();
                    if (player != null)
                    {
                        player.TakeDamage(damage, gameObject, dmgEffect);
                    }
                }
            }
            lastAttackTime = Time.time;
        }
    }

    public override void FixedUpdate()
    {
        if (player != null)
        {
            if (freeze)
            {
                if (frzTmr <= 0.0f) freeze = false;
                else
                {
                    frzTmr -= Time.deltaTime;
                    return;
                }
            }

            if (canTurnTransparent && !isTransparent)
            {
                if (Random.Range(0, 100) < transparentChance)
                {
                    StartTransparent();
                }
            }

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
        }
    }

    void StartTransparent()
    {
        isTransparent = true;
        GetComponent<Collider2D>().enabled = false;
        Color spriteColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0.4f);
        Invoke("StopTransparent", 2f);
    }

    void StopTransparent()
    {
        GetComponent<Collider2D>().enabled = true;
        Color spriteColor = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1f);
        isTransparent = false;
    }

    private float delayTime = 0.0f;
    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, obstacleDetectSize, direction, detectionDistance, obstacleMask);
        if (hit.collider != null && !hit.collider.GetComponent<Player>())
        {
            Vector2 newDirection = direction + Vector2.Perpendicular(direction)*2;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + newDirection, (isConfused ? moveSpeed / 2 : moveSpeed) * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, (isConfused ? moveSpeed / 2 : moveSpeed) * Time.deltaTime);
        }

        if (firstSprite == null || secondSprite == null) return;

        if (delayTime < walkAnimTime) delayTime += Time.deltaTime;
        else if (delayTime >= walkAnimTime)
        {
            if (animInd == 0)
            {
                spriteRenderer.sprite = firstSprite;
                animInd = 1;
            }
            else
            {
                spriteRenderer.sprite = secondSprite;
                animInd = 0;
            }
            delayTime = 0.0f;
        }
    }

    private void RandomiseProperties()
    {
        health = Random.Range(minHealth, maxHealth);
        attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
    }

    public override void TakeDamage(float damage, GameObject sender = null, DamageEffects damageEffect = DamageEffects.Nothing)
    {
        base.TakeDamage(damage, sender, damageEffect);
        if (damageEffect == DamageEffects.FreezeInPlace)
        {
            freeze = true;
            frzTmr = freezeTimer;
        }
        if (damageEffect == DamageEffects.SetOnFire)
        {
            Instantiate(fireEffectParticles, transform);
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
