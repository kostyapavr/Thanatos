using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float minHealth;
    public new float maxHealth;

    public float attackIntervalMin;
    public float attackIntervalMax;

    public float damage;

    public float moveSpeedMin;
    public float moveSpeedMax;

    [HideInInspector] public Transform player;
    
    public float attackRange;

    private float lastAttackTime;

    private float health;
    private float attackInterval;
    private float moveSpeed;

    public override void Start()
    {
        RandomiseProperties();

        currentHealth = health;
        InvokeRepeating("Attack", attackInterval, attackInterval);
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                        player.TakeDamage(damage);
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
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void RandomiseProperties()
    {
        health = Random.Range(minHealth, maxHealth);
        attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
    }
}
