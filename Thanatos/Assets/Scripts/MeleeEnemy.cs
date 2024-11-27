using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public new float maxHealth;
    public Transform attackPoint;

    public float attackInterval = 2;
    [HideInInspector] public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 1f;

    private float lastAttackTime;

    public override void Start()
    {
        currentHealth = maxHealth;
        InvokeRepeating("Attack", attackInterval, attackInterval);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + attackInterval)
        {
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
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}
